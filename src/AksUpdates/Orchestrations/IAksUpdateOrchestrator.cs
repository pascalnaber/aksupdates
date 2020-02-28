using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AksUpdates.Orchestrations
{
    public interface IAksUpdateOrchestrator
    {
        Task Run(ILogger log);
    }
}