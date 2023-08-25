namespace tyenda_backend.App.Services.Notification_Service
{
    public interface INotificationService
    {
        public abstract void SendNotificationAsync(string recipientId, string? title, string? message, string? url);
        public abstract void SendNotificationToAllAsync(string? title, string? message, string? url);
    }
}