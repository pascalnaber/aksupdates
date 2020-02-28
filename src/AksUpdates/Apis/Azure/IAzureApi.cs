using System.Threading.Tasks;

namespace AksUpdates.Apis.Azure
{
    public interface IAzureApi
    {
        Task<string> GetAksLocations();

        Task<string> GetAksVersionsByLocation(string location);
    }
}