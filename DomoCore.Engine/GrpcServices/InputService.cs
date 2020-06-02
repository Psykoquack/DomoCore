using DomoCore.Shared.GrpcProtos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.GrpcServices
{
    public class InputService : Input.InputBase
    {
        public override Task<InputReply> ReportInputs(InputValue value, ServerCallContext context)
        {
            Console.WriteLine($"Received {value.Value:X8}");
            return Task.FromResult(new InputReply { Done = true }); 
        }


    }
}
