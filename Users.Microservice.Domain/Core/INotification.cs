using static Users.Microservice.Domain.Core.NotificationModel;

namespace Users.Microservice.Domain.Core
{
    public interface INotification
    {
        NotificationModel NotificationModel { get; }
        bool HasNotification { get; }
        void AddNotification(string key, string message, ENotificationType notificationType);

    }
}
