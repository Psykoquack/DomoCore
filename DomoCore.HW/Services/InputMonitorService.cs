using Grpc.Net.Client;
using Microsoft.Extensions.ObjectPool;
using DomoCore.Shared.GrpcProtos;
using System;
using System.Timers;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DomoCore.HW.MyServices
{
    public class InputMonitorService : IHostedService, IDisposable
    {

        // Fields
        private readonly HWInterface hw;
        private readonly ILogger<InputMonitorService> logger;
        private uint currentInputs  = 0x0000;
        private uint previousInputs = 0x0000;
        private Input.InputClient client;
        private System.Timers.Timer timer;

        public InputMonitorService(HWInterface hw, ILogger<InputMonitorService> logger)
        {
            this.hw = hw;
            this.logger = logger;

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5003", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Input.InputClient(channel);
            
        }

        public Task StartAsync(CancellationToken stopToken)
        {
            logger.LogInformation("Starting input monitoring");

            hw.Init();

            timer = new System.Timers.Timer(50);
            timer.Elapsed += CheckInputs;
            timer.AutoReset = true;
            timer.Start();

            logger.LogInformation("Input monitoring started");

            return Task.CompletedTask;

            
        }


        private async void CheckInputs(object sender, ElapsedEventArgs e)
        {
            currentInputs = hw.ReadInputs(logger);

            if (currentInputs != previousInputs)
            {
                // Report to Engine
                Console.WriteLine($"Reporting {currentInputs:X8}");
                InputReply reply = await client.ReportInputsAsync(new InputValue { Value = currentInputs });
            }

            previousInputs = currentInputs;
        }


        public Task StopAsync(CancellationToken stopToken)
        {
            logger.LogInformation("Stopping input monitoring");

            timer?.Stop();

            logger.LogInformation("Input monitoring stopped");

            return Task.CompletedTask;
        }


        public void Dispose()
        {
            timer?.Dispose();
        }

    }
}
