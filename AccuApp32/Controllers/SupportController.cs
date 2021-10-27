using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using WebDB;

namespace AccuApp32MVC.Controllers
{
    public class SupportController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly WebdbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public SupportController(ILogger<HomeController> logger, WebdbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager=userManager;
        }

        public IActionResult Index()
        {
            Dictionary<int, string> vendorsList = new Dictionary<int, string>();

            var vendors = _context.Vendors.OrderBy(t=>t.Name).ToList();

            foreach (var v in vendors)
                vendorsList.Add(v.VendorID, v.Name);

            ViewBag.VendorsList = vendorsList;

            return View("support");
        }
        public IActionResult TicketsJson(int? ticketStatus,int? ticketType)
        {
            var u = _userManager.GetUserAsync(User).Result;

            //var userId = _userManager.GetUserAsync(User).Result.Id;

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var tickets = _context.Tickets.Include(t => t.Account)
                .Where(t => t.EnteredBy == User.Identity.Name || (User.IsInRole("Account Management") && t.TicketType == 0) || (User.IsInRole("TicketAdmin") && t.TicketType != 0))
                .Select(t => new
                {
                    t.TicketID,
                    t.TicketStatus,
                    TicketStatusText = t.TicketStatus == 0 ? "New" : (t.TicketStatus == 1 ? "In-Progress" : (t.TicketStatus == 2 ? "Complete" : "")),
                    t.TicketType,
                    TicketTypeText = t.TicketType == 0 ? "Emr Issue" : (t.TicketType == 1 ? "Desktop Issue" : (t.TicketType == 2 ? "Account Issue" : (t.TicketType == 3 ? "Hardware Issue" : (t.TicketType == 4 ? "Portal Issue" : (t.TicketType == 5 ? "Auto-Print Issue" : (t.TicketType == 6 ? "LIS Issue" : "Unknown")))))),
                    AccountID=t.AccountID??-1,
                    Name = t.Account!=null?t.Account.Name:string.Empty,
                    t.EnteredBy,
                    Info = t.AdditionalContactInfo,
                    t.DateEntered,
                    t.LastModified
                });

            var recordsTotal = tickets.Count();
            var recordsFiltered = 0;

            if (ticketStatus.HasValue)
                tickets = tickets.Where(t => t.TicketStatus == ticketStatus.Value);
            if (ticketType.HasValue)
                tickets = tickets.Where(t => t.TicketType == ticketType.Value);


            if (!string.IsNullOrEmpty(searchValue))
            {
                tickets = tickets.Where(t => Convert.ToString(t.TicketID).Contains(searchValue) || Convert.ToString(t.AccountID).Contains(searchValue) || t.EnteredBy.Contains(searchValue) || t.Name.Contains(searchValue)  || t.Info.Contains(searchValue));
            }

            recordsFiltered = tickets.Count();

            if (orderColumn == 0)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.TicketID) : tickets.OrderByDescending(t => t.TicketID);
            }
            if (orderColumn == 1)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.TicketStatus) : tickets.OrderByDescending(t => t.TicketStatus);
            }
            if (orderColumn == 2)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.TicketType) : tickets.OrderByDescending(t => t.TicketType);
            }
            if (orderColumn == 3)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.AccountID) : tickets.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 4)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.Name) : tickets.OrderByDescending(t => t.Name);
            }
            if (orderColumn == 5)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.EnteredBy) : tickets.OrderByDescending(t => t.EnteredBy);
            }
            if (orderColumn == 6)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.Info) : tickets.OrderByDescending(t => t.Info);
            }
            if (orderColumn == 7)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.DateEntered) : tickets.OrderByDescending(t => t.DateEntered);
            }
            if (orderColumn == 8)
            {
                tickets = (orderDirect == "asc") ? tickets.OrderBy(t => t.LastModified) : tickets.OrderByDescending(t => t.LastModified);
            }

            var ticketsList = tickets.Skip(start).Take(length).Select(t => new
            {
                t.TicketID,
                t.TicketStatus,
                TicketStatusText = t.TicketStatus == 0 ? "New" : (t.TicketStatus == 1 ? "In-Progress" : (t.TicketStatus == 2 ? "Complete" : "")),
                t.TicketType,
                TicketTypeText = t.TicketType == 0 ? "Emr Issue" : (t.TicketType == 1 ? "Desktop Issue" : (t.TicketType == 2 ? "Account Issue" : (t.TicketType == 3 ? "Hardware Issue" : (t.TicketType == 4 ? "Portal Issue" : (t.TicketType == 5 ? "Auto-Print Issue" : (t.TicketType == 6 ? "LIS Issue" : "Unknown")))))),
                t.AccountID,
                Name=t.Name,
                t.EnteredBy,
                Info=t.Info,
                t.DateEntered,
                t.LastModified
            }).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = ticketsList
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetTicket(int? id)
        {
            var currentUser = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var roles = _userManager.GetRolesAsync(currentUser).Result.OrderBy(t => t).ToList();

            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketID == id);
            if (ticket == null)
            {
                ticket = new Ticket
                {
                    TicketAttachments = new List<TicketAttachment>(),
                    TicketActions = new List<TicketAction>(),
                    TicketStatus = 0,
                    EnteredBy=User.Identity.Name,
                    TicketID=-1,
                    AccountID=null,
                    TicketType=-1
                };
            } 

            var ticketActions = _context.TicketActions.Where(t => t.TicketID == ticket.TicketID)
                .OrderByDescending(t => t.ActionDate)
                .Select(t=>new { 
                            t.TicketActionID,
                            t.ActionBy,
                            t.ActionDate,
                            t.ActionTaken
                        }).ToList();

            var repClaims = User.Claims.Where(t => t.Type == "SalesRep").Select(t => t.Value).ToList();
            var accClaims = User.Claims.Where(t => t.Type == "Account").Select(t => t.Value).ToList();
            var grpClaims = User.Claims.Where(t => t.Type == "Group").Select(t => t.Value).ToList();

            var attachments = _context.TicketAttachments.Join(_context.Attachments, l1 => l1.AttachmentID, r1 => r1.AttachmentID, (l2, r2) => new { l2, r2 })
                               .Where(t => t.l2.TicketID == id)
                               .Select(t => new
                               {
                                   t.r2.AttachmentID,
                                   t.r2.AttachmentName,
                                   t.r2.AttachmentContent
                               }).OrderBy(t => t.AttachmentName).ToList();

            var contacts = _context.TicketContacts.Where(t=>t.TicketID==id).Select(t =>new
            {
                Id=t.TicketContactID,
                Email=t.TicketContactEmail
            }).OrderBy(t => t.Email).ToList();

            var accounts = _context.Accounts.Where(t=>(repClaims.Contains(t.Group.Manager) || grpClaims.Contains(t.Group.SalesRep) || accClaims.Contains(t.AccountID.ToString()) || t.AccountID == ticket.AccountID))
                .Select(t=>new { 
                    t.AccountID,
                    t.Active,
                    t.Address,
                    t.City,
                    t.Fax,
                    t.GroupID,
                    t.Name,
                    t.NPI,
                    t.State,
                    t.SubGroup,
                    t.Suite,
                    t.Telephone,
                    t.Zip
                }).ToList();

            var data = new
            {
                TicketId=ticket.TicketID,
                DateEntered=ticket.DateEntered,
                TicketStatus=ticket.TicketStatus,
                Attachments= attachments,
                TicketActions=ticketActions,
                Accounts=accounts,
                Contacts=contacts,
                AccountId=ticket.AccountID,
                EnteredBy=ticket.EnteredBy,
                AdditionalContactInfo=ticket.AdditionalContactInfo,
                TicketType=ticket.TicketType,
                TicketDescription=ticket.TicketDescription,
                VendorId=ticket.VendorID,
                Examples=ticket.Examples,
                Roles=roles
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult SaveTicket(int? ticketId,DateTime dateEntered,string enteredBy,int ticketStatus,int ticketType,string additionalContactInfo,
                string ticketDescription,int? vendorId,int? accountId,string examples,string actionTaken,
                int[] contact,string[] email,int[] attachment,string[] content,string[] attachName)
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketID == ticketId);
            if (ticket != null)
            {
                if(User.IsInRole("Admin") || User.IsInRole("Account Manager"))
                    ticket.TicketStatus = ticketStatus;
                ticket.TicketType = ticketType;
                ticket.AdditionalContactInfo = additionalContactInfo??string.Empty;
                ticket.TicketDescription = ticketDescription;
                ticket.VendorID = vendorId;
                ticket.AccountID = accountId;
                ticket.Examples = examples;
            } else
            {
                ticket = new Ticket
                {
                    AccountID = accountId,
                    AdditionalContactInfo = additionalContactInfo ?? string.Empty,
                    DateEntered = DateTime.Now,
                    EnteredBy = User.Identity.Name,
                    LastModified = DateTime.Now,
                    Examples = examples,
                    TicketStatus=0,
                    TicketType=ticketType,
                    TicketDescription=ticketDescription,
                    VendorID=vendorId
                };
                _context.Tickets.Add(ticket);
            }

            try
            {
                _context.SaveChanges();
            } catch(Exception ex)
            {

            }

            ticketId = ticket.TicketID;

            //
            if (!string.IsNullOrEmpty(actionTaken))
            {
                var ticketAction = new TicketAction
                {
                        ActionBy=User.Identity.Name,
                        ActionTaken=actionTaken,
                        ActionDate=DateTime.Now,
                        TicketID=ticketId.Value
                };
                _context.TicketActions.Add(ticketAction);
            }
            //contacts
            var contactsDb = _context.TicketContacts.Where(t => t.TicketID == ticketId).Select(t => t.TicketContactID).ToArray();
            var delContac = contactsDb.Except(contact).ToArray();
            var delContactDb = _context.TicketContacts.Where(t => delContac.Contains(t.TicketContactID)).ToList();
            _context.TicketContacts.RemoveRange(delContactDb);

            if (email.Length > 0)
            {
                var emails = email.Select(t => new TicketContact
                {
                    TicketContactEmail = t,
                    TicketID = ticketId.Value
                }).ToList();
                _context.TicketContacts.AddRange(emails);
            }
            //attachments
            var attachDb = _context.TicketAttachments.Join(_context.Attachments,l1=>l1.AttachmentID,r1=>r1.AttachmentID,(l2,r2)=>new {l2,r2})
                            .Where(t => t.l2.TicketID == ticketId).Select(t => t.r2.AttachmentID).ToArray();
            var delAttach = attachDb.Except(attachment).ToArray();
            var delAttachDb = _context.Attachments.Where(t => delAttach.Contains(t.AttachmentID)).ToList();
            var delTicketAttachDb = _context.TicketAttachments.Where(t => delAttach.Contains(t.AttachmentID)).ToList();

            _context.TicketAttachments.RemoveRange(delTicketAttachDb);
            _context.Attachments.RemoveRange(delAttachDb);

            _context.SaveChanges();

            //add new entity
            //attach
            if (content.Length > 0)
            {
                for(var i=0;i< content.Length;i++)
                {
                    var attach = new Attachment
                    {
                        AttachmentName = attachName[i],
                        AttachmentContent = content[i],
                    };
                    _context.Attachments.Add(attach);
                    _context.SaveChanges();

                    var ticketAttach = new TicketAttachment
                    {
                          AttachmentID=attach.AttachmentID,
                          TicketID=ticket.TicketID
                    };
                    _context.TicketAttachments.Add(ticketAttach);
                    _context.SaveChanges();
                }
            }

            return Content(JsonConvert.SerializeObject("Ok"));
        }
    }
}
