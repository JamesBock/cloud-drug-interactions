using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LockStepBlazor.Data.Services
{
    public class RxCuisNotificationDispatcher : BackgroundService
    {
        private readonly Channel<string> channel;
        private readonly ILogger<RxCuisNotificationDispatcher> logger;

        public RxCuisNotificationDispatcher(Channel<string> channel, ILogger<RxCuisNotificationDispatcher> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!channel.Reader.Completion.IsCompleted)
            {
                try
                {
                    var msg = await channel.Reader.ReadAsync();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "notification failed");
                }
            }
        }
    }
}