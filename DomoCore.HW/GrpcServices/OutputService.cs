using DomoCore.HW.MyServices;
using DomoCore.Shared.GrpcProtos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.HW.GrpcServices
{
    public class OutputService : Output.OutputBase
    {
        private readonly HWInterface hwInterface;

        public OutputService(HWInterface hwInterface)
        {
            this.hwInterface = hwInterface;
        }

        public override Task<OutputReply> SetOutputs(OutputValue value, ServerCallContext context)
        {
            hwInterface.SetOutputs(value.Value);
            return Task.FromResult(new OutputReply { Done = true });
        }


        public override Task<OutputReply> KeepOutputAlive(KeepOutputAliveMessage message, ServerCallContext context)
        {
            return Task.FromResult(new OutputReply { Done = true });
        }
    }
}
