using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App.Domain.Services
{
    public class NotificationService : INotificationService
    {

        private NotificationService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        public void NotifyApproachingPayInLimit(string emailAddress)
        {
            Console.WriteLine("Email send from NotifyApproachingPayInLimit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        public void NotifyFundslow(string emailAddress)
        {
            //
            Console.WriteLine("Email send from NotifyFundsLow");
        }
    }
}
