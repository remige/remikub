namespace remikub.Hubs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public interface INotifier
    {
        Task NotifyUserHasPlayed(Guid gameId, string user);
    }

    public class Notifier : INotifier
    {
        private readonly IHubContext<NotificationHub> _notificationHub;

        public Notifier(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public async Task NotifyUserHasPlayed(Guid gameId, string user)
        {
            await _notificationHub.Clients.All.SendAsync("UserHasPlayed", new UserHasPlayed(gameId, user));
        }
    }

    public class NotificationHub : Hub
    {
        public const string Route = "/notify";
    }
}
