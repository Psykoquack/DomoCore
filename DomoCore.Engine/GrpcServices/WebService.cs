using DomoCore.Shared.GrpcProtos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.GrpcServices
{
    public class WebService : Web.WebBase
    {
        public override Task<WebInputReply> WebReportInputs(Empty empty, ServerCallContext context)
        {
           
            return Task.FromResult(new WebInputReply { Value = 0x0001000 });
        }


        public override Task<WebOutputReply> WebReportOutputs(Empty empty, ServerCallContext context)
        {

            return Task.FromResult(new WebOutputReply { Value = 0x0001000 });
        }


    }
}
