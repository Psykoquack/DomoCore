using DomoCore.Engine.Data;
using DomoCore.Shared.GrpcProtos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DomoCore.Engine.GrpcServices
{
    public class InputService : Input.InputBase
    {
        private readonly ILogger<InputService> logger;
        private readonly InMemDbContextFactory contextFactory;

        public InputService(ILogger<InputService> logger, InMemDbContextFactory contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }


        public override Task<InputReply> KeepInputAlive(KeepInputAliveMessage message, ServerCallContext context)
        {
            return Task.FromResult(new InputReply { Done = true });
        }


        public override  Task<InputReply> ReportInputs(InputValue value, ServerCallContext context)
        {
            logger.LogDebug($"Received {value.Value:X8} from {value.Sender}");

            using (DomoCoreInMemDbContext dbcontext = contextFactory.CreateContext())
            {
                foreach (var input in dbcontext.Inputs.Include(x => x.Device)
                        .Where(x => x.Device.Name == value.Sender))
                {
                    if (input.Changed == false)
                    {
                        input.PreviousState = input.CurrentState;
                        if ((value.Value != 0) && ((value.Value & (uint)input.HWValue) == value.Value))
                        {
                            logger.LogDebug($"{input.Id} Pressed");
                            input.CurrentState = Models.InputState.Pressed;
                        }
                        else
                        {
                            input.CurrentState = Models.InputState.Released;
                            if (input.PreviousState == Models.InputState.Pressed)
                            {
                                logger.LogDebug($"{input.Id} Released");
                            }
                        }
                        if (input.PreviousState != input.CurrentState)
                        {
                            input.Changed = true;
                        }
                    }
                }
                dbcontext.SaveChanges();
            }

            return Task.FromResult(new InputReply { Done = true });
        }


    }
}
