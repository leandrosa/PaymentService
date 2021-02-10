using System.Collections.Generic;
using Payments.Domain.Common;

namespace Payments.Application.Interfaces
{
    public interface INotificationContext
    {
        bool HasNotifications { get; }

        IReadOnlyCollection<Notification> Notifications { get; }

        void AddNotification(string key, string message);

        void AddNotification(Notification notification);

        void AddNotifications(IEnumerable<Notification> notifications);
    }
}
