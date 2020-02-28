using System;
using System.Threading.Tasks;
using AksUpdates.Orchestrations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AksUpdates
{
    public class TimerTriggeredAksUpdatesFunction
    {
        //[FunctionName(nameof(TimerTriggeredAksUpdatesFunction))]
        //public async Task Run([TimerTrigger("0 0 */1 * * *", RunOnStartup = true)]
        //    TimerInfo myTimer, 
        //    [DurableClient] IDurableOrchestrationClient client, 
        //    ILogger logger)
        //{
        //    await client.StartNewAsync(nameof(AksUpdateOrchestrator), null);
        //}

        private readonly IAksUpdateOrchestrator orchestrator;
        public TimerTriggeredAksUpdatesFunction(IAksUpdateOrchestrator orchestrator)
        {
            this.orchestrator = orchestrator;
        }

        [FunctionName(nameof(TimerTriggeredAksUpdatesFunction))]
        public async Task Run([TimerTrigger("0 0 */1 * * *", RunOnStartup = true)]
            TimerInfo myTimer,            
            ILogger logger)
        {
            await orchestrator.Run(logger);
        }
    }
}
