using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;


namespace AccuApp32MVC.Models
{
    public class RoleNotificationView
    {
        public List<AspNetRole> Roles { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<RoleNotification> RoleNotifications { get; set; }
    }
}
