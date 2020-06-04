using DomoCore.Engine.Data;
using DomoCore.Engine.Services;
using DomoCore.Shared.GrpcProtos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomoCore.Engine.Classes
{
    public class DomoEngine : IDisposable
    {

        [Inject]
        public InMemDbContextFactory contextFactory { get; set; }

        [Inject]
        public ILogger logger { get; set; }

        //private ILogger<EngineService> logger;
        private DomoCoreDbContext context;

        // Fields
        private Output.OutputClient client;
        private static uint counter = 0x00000001;
        //

        // Constructors      
        public DomoEngine()
        {
            //this.logger = logger;

            // GRPC setup
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Output.OutputClient(channel);

            contextFactory = new InMemDbContextFactory();

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
                                State = input.State 
                            });
                    }

                    foreach (var output in context.Outputs)
                    {
                        await memContext.Outputs.AddAsync(
                            new Models.Output 
                            { 
                                Id = output.Id, 
                                Name = output.Name, 
                                State = output.State, 
                                HWValue = output.HWValue 
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

                    await memContext.SaveChangesAsync();
                }
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopToken"></param>
        internal async void EngineLogic(CancellationToken stopToken)
        {
            await Initialize();

            using (var context = contextFactory.CreateContext())
            {
                while (!stopToken.IsCancellationRequested)
                {
                    await client.SetOutputsAsync(new OutputValue { Value = counter });
                    //logger.LogInformation($"Wrote {counter:X8}");
                    if (counter == 0x80000000)
                    {
                        counter = 0x00000001;
                    }
                    else
                    {
                        counter = counter * 2;
                    }
                    foreach (var input in context.Inputs)
                    {
                        //logger.LogDebug($"-Input {input.Id} : Value {input.HWValue} : State {input.State}");
                    }

                    Thread.Sleep(500);
                }
            }





            

        }


        public void Dispose()
        {
            contextFactory.Dispose();
        }

    }
}
