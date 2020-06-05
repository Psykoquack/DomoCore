using DomoCore.Engine.Classes;
using DomoCore.Engine.Data;
using DomoCore.Shared.GrpcProtos;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DomoCore.Engine.Services
{
    public class EngineService : IHostedService
    {

        private readonly ILogger<EngineService> logger;
        private readonly DomoEngine domoEngine;

        // Constructors
        public EngineService(ILogger<EngineService> logger, DomoEngine domoEngine)
        {
            this.logger = logger;
            this.domoEngine = domoEngine;
        }

        // Methods
        #region IHostedService

        public Task StartAsync(CancellationToken stopToken)
        {
            //DomoEngine engine = new DomoEngine();

            Task.Run(() => domoEngine.EngineLogic(stopToken));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stopToken)
        {
            return Task.CompletedTask;
        }

        #endregion IHosterService
    }
}
