using AksUpdates.Events;
using AksUpdates.Storage;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AksUpdates.EventHandlers
{
    public class AksVersionChangeEventHandler_SaveChanges : 
        INotificationHandler<AksNewVersionAvailableEvent>,
        INotificationHandler<AksNewLocationAvailableEvent>
    {        
        private readonly IAzureTableStorage azureTableStorage;

        public AksVersionChangeEventHandler_SaveChanges(IAzureTableStorage azureTableStorage)
        {
            this.azureTableStorage = azureTableStorage;
        }

        public async Task Handle(AksNewVersionAvailableEvent notification, CancellationToken cancellationToken)
        {
            await SaveInStorage(notification);
        }

        public async Task Handle(AksNewLocationAvailableEvent notification, CancellationToken cancellationToken)
        {
            await SaveInStorage(notification);
        }

        private async Task SaveInStorage(AksNewVersionAvailableEvent notification)
        {
            await this.azureTableStorage.AddOrUpdateLatestVersion(notification.NotificationType.NotificationTypeKey, notification.LocationKey, notification.LatestVersion.ToString());
            if (notification.NotificationType.IsPreview)
            {
                await this.azureTableStorage.AddOrUpdateLatestVersion("akspreviewall", notification.LocationKey, notification.PreviewVersions);
            }
        }
    }
}
