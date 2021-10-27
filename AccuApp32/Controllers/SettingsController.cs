using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebDB;
using AccuApp32MVC.Models;
using System.Collections.Specialized;
using DataModel.Finance;
using System.Security.Claims;

namespace AccuApp32MVC.Controllers
{
    public class SettingsController : Controller
    {
        private readonly WebdbContext _context;
        private readonly WebdbContextProcedures _contextProcedure;
        private readonly FINANCEContext _contextFinance;
        private readonly FINANCEContextProcedures _contextFinanceProcedure;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SettingsController(WebdbContext context, WebdbContextProcedures contextProcedure, FINANCEContext contextFinance,FINANCEContextProcedures contextFinanceProcedure, UserManager<IdentityUser> userInManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _contextProcedure = contextProcedure;
            _contextFinance = contextFinance;
            _contextFinanceProcedure = contextFinanceProcedure;
            _userManager = userInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Subscriptions()
        {
            var notifications = _context.Notifications.OrderBy(t => t.Name).ToList();
            var emails = _context.AspNetUsers.Where(l => l.LockoutEnabled).Select(t => t.Email).ToList();

            var userNotifications = _context.UserNotifications.ToList();
            var subscriptions = new UserNotificationView
            {
                Notifications = notifications,
                Emails = emails,
                UserNotifications = userNotifications
            };
            return View(subscriptions);
        }
        [HttpGet]
        public IActionResult SubscriptionsByRoles()
        {
            var notifications = _context.Notifications.OrderBy(t => t.Name).ToList();
            var roles = _context.AspNetRoles.OrderBy(t => t.Name).ToList();
            var roleNotifications = _context.RoleNotifications.ToList();


            var subscriptions = new RoleNotificationView
            {
                Notifications = notifications,
                Roles = roles,
                RoleNotifications = roleNotifications
            };
            return View("SubscriptionsByRoles", subscriptions);
        }
        [HttpPost]
        public IActionResult AddNotificationToRole(string roleId, long notificationId)
        {
            var roleNotification = _context.RoleNotifications.FirstOrDefault(t => t.RoleId == roleId && t.NotificationId == notificationId);

            if (roleNotification == null)
            {
                //add notification to role in table
                roleNotification = new RoleNotification
                {
                    RoleId = roleId,
                    NotificationId = notificationId
                };
                _context.RoleNotifications.Add(roleNotification);

                //add notification to existing
                //select dont't blocked users 
                var nonBllockUsers = _context.AspNetUserRoles.ToList().Join(_context.AspNetUsers.Where(t => t.LockoutEnabled), l => l.UserId, r => r.Id, (l, r) => l).ToList();

                var emailsInRole = nonBllockUsers.Where(t => t.RoleId == roleId).Select(t => t.UserId).Join(_context.AspNetUsers, l => l, r => r.Id, (l, r) => r).Select(t => t.Email).Distinct().ToList();

                var existsSubscriptions = _context.UserNotifications.Where(t => t.NotificationId == notificationId).ToList().Join(emailsInRole, l => l.UserId, r => r, (l, r) => l).Select(t => t.UserId).ToList();
                var newSubscriptions = emailsInRole.Except(existsSubscriptions).Select(t => new UserNotification
                {
                    NotificationId = notificationId,
                    UserId = t
                }).ToList();

                _context.UserNotifications.AddRange(newSubscriptions);
            }

            _context.SaveChanges();
            return Content(JsonConvert.SerializeObject("OK"));
        }
        [HttpPost]
        public IActionResult RemoveNotificationFromRole(string roleId, long notificationId)
        {
            var roleNotification = _context.RoleNotifications.FirstOrDefault(t => t.RoleId == roleId && t.NotificationId == notificationId);

            if (roleNotification != null)
            {
                //remove notification from role in table
                _context.RoleNotifications.Remove(roleNotification);

                //remove notification to existing
                var emailsInRole = _context.AspNetUserRoles.Where(t => t.RoleId == roleId).Select(t => t.UserId).Join(_context.AspNetUsers, l => l, r => r.Id, (l, r) => r).Select(t => t.Email).Distinct().ToList();

                var existsSubscriptions = _context.UserNotifications.Where(t => t.NotificationId == notificationId).ToList().Join(emailsInRole, l => l.UserId, r => r, (l, r) => l).ToList();

                _context.UserNotifications.RemoveRange(existsSubscriptions);
            }

            _context.SaveChanges();
            return Content(JsonConvert.SerializeObject("OK"));
        }
        // notification for service mail
        [HttpGet]
        public IActionResult SubscriptionsByServiceEmail()
        {
            var notifications = _context.Notifications.OrderBy(t => t.Name).ToList();
            var serviceEmails = _context.ServiceEmails.OrderBy(t => t.Email).ToList();
            var serviceemailNotifications = _context.ServiceNotifications.ToList();


            var subscriptions = new ServiceNotificationView
            {
                ServiceEmails = serviceEmails,
                Notifications = notifications,
                ServiceNotifications = serviceemailNotifications
            };
            return View("SubscriptionsByServiceNotifications", subscriptions);
        }
        [HttpPost]
        public IActionResult AddNotificationToServiceEmail(string email, long notificationId)
        {
            var serviceNotification = _context.ServiceNotifications.FirstOrDefault(t => t.Email == email && t.NotificationId == notificationId);

            if (serviceNotification == null)
            {
                //add notification to role in table
                serviceNotification = new ServiceNotification
                {
                    Email = email,
                    NotificationId = notificationId
                };
                _context.ServiceNotifications.Add(serviceNotification);
                _context.SaveChanges();
            }
            return Content(JsonConvert.SerializeObject("OK"));
        }
        [HttpPost]
        public IActionResult RemoveNotificationFromServiceEmail(string email, long notificationId)
        {
            var serviceNotification = _context.ServiceNotifications.FirstOrDefault(t => t.Email == email && t.NotificationId == notificationId);

            if (serviceNotification != null)
            {
                //remove notification from role in table
                _context.ServiceNotifications.Remove(serviceNotification);
                _context.SaveChanges();
            }
            return Content(JsonConvert.SerializeObject("OK"));
        }

        //============================
        [HttpPost]
        public IActionResult AddNotificationSubscribe(string email, long notificationId)
        {
            var subscribe = _context.UserNotifications.FirstOrDefault(t => t.UserId == email && t.NotificationId == notificationId);

            if (subscribe == null)
            {
                subscribe = new UserNotification
                {
                    UserId = email,
                    NotificationId = notificationId
                };

                _context.UserNotifications.Add(subscribe);
                _context.SaveChanges();

            }
            return Content(JsonConvert.SerializeObject("OK"));
        }

        [HttpPost]
        public IActionResult RemoveNotificationSubscribe(string email, long notificationId)
        {
            var subscribe = _context.UserNotifications.Where(t => t.UserId == email && t.NotificationId == notificationId).ToList();

            if (subscribe != null && subscribe.Count > 0)
            {
                _context.UserNotifications.RemoveRange(subscribe);
                _context.SaveChanges();

            }
            return Content(JsonConvert.SerializeObject("OK"));
        }

        [HttpGet]
        public IActionResult Notifications()
        {
            var notifications = _context.Notifications.OrderBy(t => t.Name).ToList();
            return View(notifications);
        }
        [HttpPost]
        public IActionResult AddNotification(string name, string description)
        {
            var bExists = _context.Notifications.Any(t => t.Name.ToLower() == name.ToLower());

            if (bExists)
                return Content(JsonConvert.SerializeObject("Notification is exists"));
            else
            {
                var notification = new Notification
                {
                    Name = name,
                    Description = description
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
        }
        [HttpPost]
        public IActionResult EditNotification(long notificationId, string notifyName, string notifyDescription)
        {
            var notification = _context.Notifications.FirstOrDefault(t => t.NotificationId == notificationId);

            if (notification != null)
            {
                notification.Name = notifyName;
                notification.Description = notifyDescription;

                _context.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("Error"));
            }
        }
        [HttpPost]
        public IActionResult DelNotification(long notificationId)
        {
            var subscriptions = _context.UserNotifications.Any(t => t.NotificationId == notificationId);
            var notification = _context.Notifications.FirstOrDefault(t => t.NotificationId == notificationId);

            if (notification == null || subscriptions)
                return Content(JsonConvert.SerializeObject("Error"));
            else
            {
                _context.Notifications.Remove(notification);
                _context.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
        }
        [HttpGet]
        public IActionResult ServiceEmails()
        {
            var serviceEmails = _context.ServiceEmails.OrderBy(t => t.Email).ToList();
            return View(serviceEmails);
        }
        [HttpPost]
        public IActionResult AddServiceEmail(string email, string description)
        {
            var bExists = _context.ServiceEmails.Any(t => t.Email.ToLower() == email.ToLower());

            if (bExists)
                return Content(JsonConvert.SerializeObject("Email is exists"));
            else
            {
                var serviceEmail = new ServiceEmail
                {
                    Email = email,
                    Description = description
                };

                _context.ServiceEmails.Add(serviceEmail);
                _context.SaveChanges();

                return Content(JsonConvert.SerializeObject("OK"));
            }
        }
        [HttpPost]
        public IActionResult EditServiceEmail(string email, string Description)
        {
            var serviceEmail = _context.ServiceEmails.FirstOrDefault(t => t.Email.ToLower() == email.ToLower());

            if (serviceEmail != null)
            {
                serviceEmail.Description = Description;

                _context.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("Error"));
            }
        }
        [HttpPost]
        public IActionResult DelServiceEmail(string email)
        {
            var serviceEmail = _context.ServiceEmails.FirstOrDefault(t => t.Email.ToLower() == email.ToLower());

            if (serviceEmail == null)
                return Content(JsonConvert.SerializeObject("Error"));
            else
            {
                _context.ServiceEmails.Remove(serviceEmail);
                _context.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
        }
        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UsersJson()
        {
            //get parameters

            var userId = _userManager.GetUserAsync(User).Result.Id;

            var draw=Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue= Request.Form["search[value]"].ToString();

            long i = 1;

            var users = _context.AspNetUsers.GroupJoin(_context.RegistrationDetails, l1 => l1.Id, r1 => r1.UserID, (l2, r2) => new { r2, l2 })
                        .SelectMany(l3=>l3.r2.DefaultIfEmpty(),(l4,r4)=>new {l4,r4 }).Select(t =>new 
                        {
                            UserId=t.l4.l2.Id,
                            Email = t.l4.l2.Email,
                            FirstName = t.r4.FirstName,
                            LastName = t.r4.LastName
                        });

            var recordsTotal = users.Count();
            var recordsFiltered = 0;

            if (!string.IsNullOrEmpty(searchValue))
            {
                users = users.Where(t => t.FirstName.Contains(searchValue) || t.LastName.Contains(searchValue) || t.Email.Contains(searchValue));
                recordsFiltered= users.Count();
            } else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                users = (orderDirect == "asc") ? users.OrderBy(t => t.LastName) : users.OrderByDescending(t => t.LastName);
            }
            if (orderColumn == 1)
            {
                users = (orderDirect == "asc") ? users.OrderBy(t => t.FirstName) : users.OrderByDescending(t => t.FirstName);
            }
            if (orderColumn == 2)
            {
                users = (orderDirect == "asc") ? users.OrderBy(t => t.Email) : users.OrderByDescending(t => t.Email);
            }


            var usersList = users.Skip(start).Take(length).Select(t=>new
            {
                t.UserId,
                t.Email,
                t.FirstName,
                t.LastName,
                RolesString = string.Join(",", _context.AspNetUserRoles.Where(l=>l.UserId==t.UserId).Join(_context.AspNetRoles, l => l.RoleId, r => r.Id, (l, r) =>new { l,r }).OrderBy(t => t.r.Name).Select(t => t.r.Name).ToArray())
            }).ToList();

            var data = new 
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = usersList,
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetUser(string userId)
        {
            var user = _context.AspNetUsers.GroupJoin(_context.RegistrationDetails, l1 => l1.Id, r1 => r1.UserID, (l2, r2) => new { r2, l2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new {l4,r4})
                .Select(t=>new
                {
                    UserId = t.l4.l2.Id,
                    Email = t.l4.l2.Email,
                    UserName=t.l4.l2.UserName,
                    FirstName = t.r4.FirstName,
                    LastName = t.r4.LastName,
                })
                .FirstOrDefault(t => t.UserId == userId);

            if (user != null)
            {
                var identityUser = new IdentityUser
                {
                    Id = user.UserId,
                    UserName = user.UserName
                };
                var userView = new UserEditView
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles=_context.AspNetRoles.ToList().GroupJoin(_context.AspNetUserRoles.Where(t=>t.UserId==userId).ToList(),l=>l.Id,r=>r.RoleId,(l,r)=>new {l,r})
                    .SelectMany(y => y.r.DefaultIfEmpty(), (l, y) => new {l,y})
                    .Select(t=>new Role
                    {
                         RoleId=t.l.l.Id,
                         Name=t.l.l.Name,
                         Check=t.y!=null
                    }).OrderBy(t=>t.Name).ToList(),
                    UserClaims=_context.AspNetUserClaims.Where(t=>t.UserId==userId).Select(t=>new UserClaim {
                          UserId=user.UserId,
                          ClaimType=t.ClaimType,
                          Value=t.ClaimValue
                    }).OrderBy(t=>t.ClaimType).ToList()
                };
                return Content(JsonConvert.SerializeObject(userView));
            } else
            {
                return Content(JsonConvert.SerializeObject("ERROR"));
            }
        }
        public void SaveUser()
        {
            var data = Request.Form;
        }
        public ActionResult ImportUsersFromFinance()
        {
            var billUsers = _contextFinance.Bill_Users.ToList();

            foreach (var u in billUsers)
            {
                u.Username = u.Username.Trim().Replace(" ", "");
                if (u.Username.Substring(u.Username.Length - 1, 1) == ".")
                    u.Username = u.Username.Substring(0, u.Username.Length - 1);

                var userWebDb = _context.AspNetUsers.FirstOrDefault(t => t.Email == u.Username + "@accureference.com");

                if (userWebDb == null)
                {
                    var appUser = new ApplicationUser
                    {
                        Email = u.Username + "@accureference.com",
                        EmailConfirmed=true,
                        UserName= u.Username + "@accureference.com"
                    };
                    var register=_userManager.CreateAsync(appUser,u.Password).Result;
                } else
                {
                    var registerdata = _context.RegistrationDetails.FirstOrDefault(t => t.UserID == userWebDb.Id);
                    if (registerdata == null)
                    {
                        var splitName = u.Name.Split(" ");
                        registerdata = new RegistrationDetail
                        {
                            FirstName = splitName.Length > 0 ? splitName[0].Trim():string.Empty,
                            LastName= splitName.Length > 1 ? splitName[1].Trim() : string.Empty,
                            UserID=userWebDb.Id
                        };
                        _context.RegistrationDetails.Add(registerdata);
                    }
                }
            }
            _context.SaveChanges();
            return Content(JsonConvert.SerializeObject("Ok"));
        }
        public IActionResult AddRoles()
        {
            string[] roles = new string[] { "Finance", "Client Services", "Phlebotomy User", "Warehouse", "Admin", "Phlebotomy Manager", "Sales Rep", "LabTech", "Client", "LabAdmin", "Coding", "FLoat User", "Account Management","BillingTools","CodingTools","BillingManagment","PLReporting", "PhlebotomyManagement","EMRManagement", "Executive","Marketing" };

            foreach (var role in roles)
            {
                var existRole = _roleManager.Roles.FirstOrDefault(t=>t.Name.ToLower()==role.ToLower());
                if (existRole == null)
                {
                    var identityRole = new IdentityRole
                    {
                         Name=role
                    };
                    var result=_roleManager.CreateAsync(identityRole).Result;
                }
            }

            return Content(JsonConvert.SerializeObject("Ok"));
        }
    }
}
