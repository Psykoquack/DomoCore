using DomoCore.Engine.Data;
using DomoCore.Engine.Models;
using DomoCore.Engine.Services;
using DomoCore.Shared.GrpcProtos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomoCore.Engine.Classes
{
    public class DomoEngine : IDisposable
    {

        // Fields
        private Shared.GrpcProtos.Output.OutputClient client;
        private int keepAliveCounter = 0;
        private readonly InMemDbContextFactory contextFactory;
        private readonly ILogger<DomoEngine> logger;
        private List<Models.Input> inputs = new List<Models.Input>();

        // Constructors      
        public DomoEngine(InMemDbContextFactory contextFactory, ILogger<DomoEngine> logger)
        {

            // GRPC setup
            // TODO: create channel for each output device
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            GrpcChannel channel = GrpcChannel.ForAddress("https://192.168.0.236:5001", 
                new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Shared.GrpcProtos.Output.OutputClient(channel);

            this.contextFactory = contextFactory;
            this.logger = logger;
        }


        // Methods

        private async Task<bool> Initialize()
        {
            // Copy data from database in memory
            using (DomoCoreDbContext context = new DomoCoreDbContext())
            {
                using (DomoCoreInMemDbContext memContext = contextFactory.CreateContext())
                {
                    foreach (var input in context.Inputs)
                    {
                        await memContext.Inputs.AddAsync(
                            new Models.Input
                            {
                                Id = input.Id,
                                HWValue = input.HWValue,
                                CurrentState = input.CurrentState,
                                PreviousState = input.PreviousState,
                                DeviceId = input.DeviceId,
                                Changed = input.Changed
                            });
                    }

                    foreach (var output in context.Outputs)
                    {
                        await memContext.Outputs.AddAsync(
                            new Models.Output
                            {
                                Id = output.Id,
                                State = output.State,
                                HWValue = output.HWValue,
                                DeviceId = output.DeviceId
                            });
                    }

                    foreach (var simpleOutput in context.SimpleOutputs)
                    {
                        await memContext.SimpleOutputs.AddAsync(
                            new Models.SimpleOutput
                            {
                                Id = simpleOutput.Id,
                                Name = simpleOutput.Name,
                                InputId = simpleOutput.InputId,
                                OutputId = simpleOutput.OutputId,
                                AutoOff = simpleOutput.AutoOff,
                                AutoOffTimeSecs = simpleOutput.AutoOffTimeSecs,
                                State = simpleOutput.State
                            });
                    }

                    foreach (var device in context.Devices)
                    {
                        await memContext.Devices.AddAsync(
                            new Models.Device
                            {
                                Id = device.Id,
                                Address = device.Address,
                                Name = device.Name
                            }
                        );
                    }

                    await memContext.SaveChangesAsync();
                }
            }

            logger.LogDebug("Database copied to memory");

            await client.SetOutputsAsync(new OutputValue { Value = 0 });
            logger.LogDebug("Outputs switched off");

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopToken"></param>
        internal async void EngineLogic(CancellationToken stopToken)
        {
            uint outputValue = 0x00000000;
            bool outputUpdate = false;

            await Initialize();

            while (!stopToken.IsCancellationRequested)
            {
                outputValue = 0x00000000;
                outputUpdate = false;

                using (var dbcontext = contextFactory.CreateContext())
                {
                    foreach (var input in dbcontext.Inputs.Where(x => x.Changed == true))
                    {
                        if (input.CurrentState == InputState.Pressed)
                        {
                            logger.LogDebug($"Input {input.Id} was pressed");
                        }
                        else
                        {
                            logger.LogDebug($"Input {input.Id} was released");
                        }

                        foreach (var simpleOutput in dbcontext.SimpleOutputs.Include(x => x.Output).Where(x => x.InputId == input.Id))
                        {
                            if (input.CurrentState == InputState.Pressed)
                            {
                                if (simpleOutput.Output.State == OutputState.Off)
                                {
                                    simpleOutput.Output.State = OutputState.On;
                                }
                                else
                                {
                                    simpleOutput.Output.State = OutputState.Off;
                                }
                                simpleOutput.Output.Changed = true;
                            }
                        }

                        input.Changed = false;
                        dbcontext.SaveChanges();

                        if (dbcontext.Outputs.Any(x => x.Changed == true))
                        {
                            foreach (var output in dbcontext.Outputs.Where(x => x.State == OutputState.On))
                            {
                                outputValue |= (uint) output.HWValue;
                            }
                            outputUpdate = true;

                            foreach (var output in dbcontext.Outputs.Where(x => x.Changed == true))
                            {
                                output.Changed = false;
                            }
                        }
                    }
                }

                if (outputUpdate)
                {
                    await client.SetOutputsAsync(new OutputValue { Value = outputValue });
                    keepAliveCounter = 0;
                }

                // Keep connection alive
                keepAliveCounter++;
                if (keepAliveCounter == 100)
                {
                    await client.KeepOutputAliveAsync(new KeepOutputAliveMessage { Receiver = "Domo1" });
                    keepAliveCounter = 0;
                }


                // Should be put in Timer
                Thread.Sleep(50);
            }
        }


        public void Dispose()
        {
            contextFactory.Dispose();
        }

    }
}
