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
using Microsoft.Extensions.Configuration;

namespace DomoCore.HW.MyServices
{
    public class InputMonitorService : IHostedService, IDisposable
    {

        // Fields
        private readonly HWInterface hw;
        private readonly ILogger<InputMonitorService> logger;
        private readonly IConfiguration configuration;
        private uint currentInputs  = 0x0000;
        private uint previousInputs = 0x0000;
        private Input.InputClient client;
        private System.Timers.Timer timer;
        private bool busy = false;
        private int keepAliveCounter = 0;

        public InputMonitorService(HWInterface hw, ILogger<InputMonitorService> logger, IConfiguration configuration)
        {
            this.hw = hw;
            this.logger = logger;
            this.configuration = configuration;
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            // Todo: get server address from appsettings.
            GrpcChannel channel = GrpcChannel.ForAddress("https://192.168.0.145:5003", new GrpcChannelOptions { HttpHandler = httpHandler });
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
            if (busy)
            {
                return;
            }

            busy = true;

            currentInputs = hw.ReadInputs(logger);

            if (currentInputs != previousInputs)
            {
                // Report to Engine
                Console.WriteLine($"Reporting {currentInputs:X8}");
                InputReply reply = await client.ReportInputsAsync(new InputValue { Value = currentInputs, Sender = configuration["Name"] });
                keepAliveCounter = 0;
            }
            previousInputs = currentInputs;

            // Keep the connection alive if there has been no activity over 100 iterations
            keepAliveCounter++;
            if (keepAliveCounter == 100)
            {
                await client.KeepInputAliveAsync(new KeepInputAliveMessage { Sender = configuration["Name"] });
                keepAliveCounter = 0;
            }

            busy = false;
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
