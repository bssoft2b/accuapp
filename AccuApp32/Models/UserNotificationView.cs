using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;

namespace AccuApp32MVC.Models
{
    public class UserNotificationView
    {
        public List<Notification> Notifications { get; set; }

        public List<string> Emails { get; set; }

        public List<UserNotification> UserNotifications { get; set; }
    }
}
