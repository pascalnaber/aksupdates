using System.Threading.Tasks;

namespace AksUpdates.Apis.Azure
{
    public interface IAzureApi
    {
        Task<string> GetAksRegions();

        Task<string> GetAksVersionsByRegion(string location);
    }
}