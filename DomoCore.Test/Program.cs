using Grpc.Net.Client;
using System;
using DomoCore.Shared.GrpcProtos;
using System.Timers;
using System.Net.Http;

namespace DomoCore.Test
{
    class Program
    {

        static Input.InputClient client;
        static uint counter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5003", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Input.InputClient(channel);

            Timer timer = new Timer(1000);
            timer.Elapsed += CheckInputs;
            timer.AutoReset = true;
            timer.Enabled = true;

            Console.WriteLine("Timer started");

            Console.ReadKey();
        }

        private static async void CheckInputs(object sender, ElapsedEventArgs e)
        {
            //currentInputs = hw.ReadInputs();
            

            //if (currentInputs != previousInputs)
            {
                // Report to Engine
                Console.WriteLine("Reporting");
                InputReply reply = await client.ReportInputsAsync(new InputValue { Value = counter });
                counter++;
            }

        }
    }
}


