using AksUpdates.Orchestrations;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AksUpdates
{
    public class TimerTriggeredAksUpdatesFunction
    {
        private readonly IAksUpdateOrchestrator _orchestrator;
        private readonly ILogger<TimerTriggeredAksUpdatesFunction> _logger;

        public TimerTriggeredAksUpdatesFunction(IAksUpdateOrchestrator orchestrator, ILogger<TimerTriggeredAksUpdatesFunction> logger)
        {
            this._orchestrator = orchestrator;
            this._logger = logger;
        }

        [FunctionName(nameof(TimerTriggeredAksUpdatesFunction))]
        public async Task Run([TimerTrigger("0 0 */1 * * *", RunOnStartup = true)]
            TimerInfo myTimer)
        {
            await _orchestrator.Run();
        }
    }
}
