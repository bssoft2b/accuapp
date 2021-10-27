using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;


namespace AccuApp32MVC.Models
{
    public class ServiceNotificationView
    {
        public List<ServiceEmail> ServiceEmails { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<ServiceNotification> ServiceNotifications { get; set; }

    }
}
