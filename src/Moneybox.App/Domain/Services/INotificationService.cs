namespace Moneybox.App.Domain.Services
{
    public interface INotificationService
    {
        void NotifyApproachingPayInLimit(string emailAddress);

        void NotifyFundslow(string emailAddress);
    }
}
