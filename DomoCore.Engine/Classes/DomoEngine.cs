using DomoCore.Engine.Services;
using DomoCore.Shared.GrpcProtos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomoCore.Engine.Classes
{
    public class DomoEngine
    {

        private ILogger<EngineService> logger;

        // Fields
        private Output.OutputClient client;
        private static uint counter = 0x00000001;

        // Constructors      
        public DomoEngine(ILogger<EngineService> logger)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Output.OutputClient(channel);
            this.logger = logger;
        }


        // Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopToken"></param>
        internal async void EngineLogic(CancellationToken stopToken)
        {
            counter = 0x00000001;
            while (!stopToken.IsCancellationRequested)
            {
                await client.SetOutputsAsync(new OutputValue { Value = counter });
                logger.LogInformation($"Wrote {counter:X8}");
                if (counter == 0x80000000)
                {
                    counter = 0x00000001;
                }
                else
                {
                    counter = counter * 2;
                }
                Thread.Sleep(500);
            }
        }

    }
}
