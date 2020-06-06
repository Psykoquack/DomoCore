using DomoCore.Shared.GrpcProtos;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomoCore.Web.Services
{
    public class StateManagement
    {
        private Input.InputClient client;


        public StateManagement()
        {
            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            GrpcChannel channel = GrpcChannel.ForAddress("https://192.168.0.145:5003", new GrpcChannelOptions { HttpHandler = httpHandler });
            client = new Input.InputClient(channel);
        }


        public async Task PressButton(uint value)
        {
            await client.ReportInputsAsync(new InputValue { Value = value, Sender = "Domo1" });
            Thread.Sleep(200); // Just for testing purposes
            await client.ReportInputsAsync(new InputValue { Value = 0, Sender = "Domo1"});
        }
    }
}
