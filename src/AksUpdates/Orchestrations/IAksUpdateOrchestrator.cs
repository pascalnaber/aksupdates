using System.Threading.Tasks;

namespace AksUpdates.Orchestrations
{
    public interface IAksUpdateOrchestrator
    {
        Task Run();
    }
}