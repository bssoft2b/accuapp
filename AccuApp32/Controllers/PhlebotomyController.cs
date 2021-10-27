using AccuApp32MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhlebotomyDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebDB;
using Account = PhlebotomyDB.Account;

namespace AccuApp32MVC.Controllers
{
    public class PhlebotomyController : Controller
    {
        private readonly WebdbContext _wdb;
        private readonly WebdbContextProcedures _contextProcedureWebDb;

        private readonly PhlebotomyContext _ph;

        private readonly UserManager<ApplicationUser> _userInManager;

        public PhlebotomyController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext contextPhlebotomy)
        {
            _wdb = contextWebDb;
            _contextProcedureWebDb = contextProcedureWebDb;
            _ph = contextPhlebotomy;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PhlebotomistsList()
        {
            return View();
        }
        public IActionResult PhlebotomistsJson(int activeStatus)
        {
            //get parameters

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();


            var phlebotomists = _ph.Phlebotomists.Select(x => x);
            var Claims=User.Claims.Where(c => c.Type == "PhlebotomyLead").Select(t => t.Value).ToList();
            //var ar = _ph.Phlebotomists.Join(_ph.PhlebotomistLeads.Where(r => Claims.Contains(r.LeadID.ToString())), l => l.EmployeeID, r => r.EmployeeID, (l, r) => l).Distinct();

            if(activeStatus==1)
                phlebotomists= phlebotomists.Where(x => (x.TerminationDate == null) || (x.TerminationDate > DateTime.Now));
            else if(activeStatus==2)
                phlebotomists = phlebotomists.Where(x => x.TerminationDate <= DateTime.Now);

            var recordsTotal = phlebotomists.Count();
            var recordsFiltered = 0;

            //filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                phlebotomists = phlebotomists.Where(x => ((x.EmployeeID??"").ToLower().Contains(searchValue)) ||
                     ((x.FirstName ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.LastName ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.State ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.Telephone ?? "").Contains(searchValue.ToLower())) ||
                     ((x.Address1 ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.Address2 ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.City ?? "").ToLower().Contains(searchValue.ToLower())) ||
                     ((x.Zip ?? "").Contains(searchValue.ToLower())));
                recordsFiltered = phlebotomists.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }
            //ordering

            if (orderColumn == 0)
            {
                phlebotomists = (orderDirect == "asc") ? phlebotomists.OrderBy(t => t.FirstName) : phlebotomists.OrderByDescending(t => t.FirstName);
            }
            if (orderColumn == 1)
            {
                phlebotomists = (orderDirect == "asc") ? phlebotomists.OrderBy(t => t.LastName) : phlebotomists.OrderByDescending(t => t.LastName);
            }
            if (orderColumn == 2)
            {
                phlebotomists = (orderDirect == "asc") ? phlebotomists.OrderBy(t => t.State) : phlebotomists.OrderByDescending(t => t.State);
            }
            if (orderColumn == 3)
            {
                phlebotomists = (orderDirect == "asc") ? phlebotomists.OrderBy(t => t.Telephone) : phlebotomists.OrderByDescending(t => t.Telephone);
            }
            if (orderColumn == 4)
            {
                phlebotomists = (orderDirect == "asc") ? phlebotomists.OrderBy(t => t.EmployeeID) : phlebotomists.OrderByDescending(t => t.EmployeeID);
            }

            var phlebotomistList = phlebotomists.Skip(start).Take(length).ToList();

            var data = new 
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = phlebotomistList,
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult GetPhlebotomist(string employeeId)
        {
            var phlebotomist = _ph.Phlebotomists.FirstOrDefault(t => t.EmployeeID == employeeId);

            if (phlebotomist == null)
            {
                return Content(JsonConvert.SerializeObject("Error"));
            }

            var leads = _ph.Leads.OrderBy(t => t.LeadName).ToList();
            var leadNames = new List<string>();

            var phlebotomistLeads = _ph.PhlebotomistLeads.Where(l => l.EmployeeID == employeeId).ToList();

            foreach(var el in phlebotomistLeads)
            {
                var lead = _ph.Leads.FirstOrDefault(t => t.LeadID == el.LeadID);
                if (lead != null)
                    leadNames.Add(lead.LeadName);
            }

            var data = new PhlebotomistView
            {
                EmployeeID = phlebotomist.EmployeeID,
                FirstName = phlebotomist.FirstName ?? "",
                LastName = phlebotomist.LastName ?? "",
                Address1 = phlebotomist.Address1 ?? "",
                Address2 = phlebotomist.Address2 ?? "",
                State = phlebotomist.State ?? "",
                City = phlebotomist.City ?? "",
                Telephone=phlebotomist.Telephone??"",
                Zip= phlebotomist.Zip??"",
                PhlebotomistLeads = _ph.PhlebotomistLeads.Where(l => l.EmployeeID == employeeId).Join(_ph.Leads, l => l.LeadID, r => r.LeadID, (l, r) => new { l, r }).Select(k => 
                new ValueName
                {
                    Id = k.l.LeadID,
                    Name = k.r.LeadName
                }).OrderBy(m => m.Name).ToList(),
                Leads=_ph.Leads.Select(t=>new ValueName
                {
                     Id=t.LeadID,
                     Name=t.LeadName
                }).OrderBy(l=>l.Name).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }

        public IActionResult GetPhlebotomistDetails(string employeeId)
        {
            var phlebotomistAndLines = _ph.Phlebotomists.Where(p => p.EmployeeID == employeeId).Join(_ph.PhlebotomistAssignments, l => l.EmployeeID, r => r.EmployeeID, (l, r) => new { l, r })
                                        .Join(_ph.PhlebotomistAssignmentLines, l => l.r.PhlebotomistAssignmentID, r => r.PhlebotomistAssignmentID, (l, r) => new { l, r })
                                        .Select(t => new
                                        {
                                            Phlebotomist = t.l.l,
                                            Assignments = t.l.r,
                                            AssignmentLines = t.r
                                        });

            var phlebotomist = phlebotomistAndLines.Select(x => x.Phlebotomist).Distinct().FirstOrDefault() ?? new Phlebotomist();
            var PhlebotomistAssignmentLine = phlebotomistAndLines.Select(x => x.AssignmentLines).Distinct().OrderBy(x => x.PhlebotomistAssignmentID).ThenByDescending(x => x.Percentage)
                .ToList() ?? new List<PhlebotomistAssignmentLine>();

            var data = new PhlebotomistView
            {
                EmployeeID = phlebotomist.EmployeeID,
                FirstName = phlebotomist.FirstName ?? "",
                LastName = phlebotomist.LastName ?? "",
                Address1 = phlebotomist.Address1 ?? "",
                Address2 = phlebotomist.Address2 ?? "",
                State = phlebotomist.State ?? "",
                City = phlebotomist.City ?? "",
                Telephone = phlebotomist.Telephone ?? "",
                Zip = phlebotomist.Zip ?? "",
                 PhlebotomistAssignmentLines= PhlebotomistAssignmentLine
            };
            return Content(JsonConvert.SerializeObject(data));
        }

        public IActionResult SavePhlebotomist(string employeeId, string firstName, string lastName, string Address1, string Address2, string state, string city, string telephone, string zip, int[] phlebotomistsLead)
        {
            var phlebotomist = _ph.Phlebotomists.FirstOrDefault(t => t.EmployeeID == employeeId);

            if (phlebotomist != null)
            {

                var oldLead = _ph.PhlebotomistLeads.Where(t => t.EmployeeID == employeeId).Select(t => t.LeadID).ToArray();

                var newLead = phlebotomistsLead.Except(oldLead);
                var delLead = oldLead.Except(phlebotomistsLead);

                var newPhlebotomistLeads = newLead.Select(t => new PhlebotomistLead
                {
                    EmployeeID = employeeId,
                    LeadID = t
                }).ToList();


                var delPhlebotomistLeads = _ph.PhlebotomistLeads.Where(t => t.EmployeeID == employeeId).ToList().Join(delLead, l => l.LeadID, r => r, (l, r) => l).ToList();

                _ph.PhlebotomistLeads.RemoveRange(delPhlebotomistLeads);
                _ph.PhlebotomistLeads.AddRange(newPhlebotomistLeads);

                phlebotomist.Address1 = Address1;
                phlebotomist.Address2 = Address2;
                phlebotomist.State = state;
                phlebotomist.City = city;
                phlebotomist.Telephone = telephone;
                phlebotomist.Zip = zip;

                _ph.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("Error"));
            }
        }

        public IActionResult DispatchTasks()
        {
            return View();
        }
        public IActionResult DispatchTasksJson(int? status)
        {
            //get parameters


            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();


            var Claims = User.Claims.Where(c => c.Type == "Account").Select(t => t.Value).ToList();
            var GrpClaims = User.Claims.Where(c => c.Type == "Group").Select(t=>t.Value).ToList();

            var tasks = _wdb.DispatchTasks.Join(_wdb.Accounts, l => l.AccountID, r => r.AccountID, (l, r) => new { l, r }).Join(_wdb.DispatchStatuses, l => l.l.DispatchStatusID, r => r.DispatchStatusID, (l, r) => new { l, r });

            if (status.HasValue && status.Value == 2)
                tasks = tasks.Where(x => x.l.l.DispatchStatusID == 1);
            else if ( status.HasValue && status.Value == 3)
                tasks = tasks.Where(x => x.l.l.DispatchStatusID == 2);

            var recordsTotal = tasks.Count();
            var recordsFiltered = 0;

            //filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                var culture =new System.Globalization.CultureInfo("en-US");
                tasks = tasks.Where(
                          x => x.l.l.DispatchTaskID.ToString().Contains(searchValue)
                     ||  (x.l.r.AccountID.ToString().Contains(searchValue))
                     ||  (x.l.r.Name.ToLower().Contains(searchValue.ToLower()))
                     ||  (x.l.r.Telephone.Contains(searchValue))
                     ||  (x.l.l.PickupTime.Contains(searchValue))
                     ||  (x.l.l.ContactName.ToLower().Contains(searchValue.ToLower()))
                     ||  (x.r.DispatchStatusName.Contains(searchValue))
                     //||  (Convert.ToString(x.l.l.LastWorked, culture).Contains(searchValue))
                     );

                recordsFiltered = tasks.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }
            //ordering
            if (orderColumn == 0)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.DispatchStatusID) : tasks.OrderByDescending(t => t.l.l.DispatchStatusID);
            }
            if (orderColumn == 1)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.DispatchTaskID) : tasks.OrderByDescending(t => t.l.l.DispatchTaskID);
            }
            if (orderColumn == 2)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.AccountID) : tasks.OrderByDescending(t => t.l.l.AccountID);
            }
            if (orderColumn == 3)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.r.Name) : tasks.OrderByDescending(t => t.l.r.Name);
            }
            if (orderColumn == 4)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.r.Telephone) : tasks.OrderByDescending(t => t.l.r.Telephone);
            }
            if (orderColumn == 5)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.Stat) : tasks.OrderByDescending(t => t.l.l.Stat);
            }
            if (orderColumn == 6)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.ReasonID) : tasks.OrderByDescending(t => t.l.l.ReasonID);
            }
            if (orderColumn == 7)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.ContactName) : tasks.OrderByDescending(t => t.l.l.ContactName);
            }
            if (orderColumn == 8)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.PickupTime) : tasks.OrderByDescending(t => t.l.l.PickupTime);
            }
            if (orderColumn == 9)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.LastWorked) : tasks.OrderByDescending(t => t.l.l.LastWorked);
            }
            if (orderColumn == 10)
            {
                tasks = (orderDirect == "asc") ? tasks.OrderBy(t => t.l.l.CreatedBy) : tasks.OrderByDescending(t => t.l.l.CreatedBy);
            }

            var tasksList = tasks.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = tasksList.Select(t=>new
                {
                    DispatchStatusId=t.l.l.DispatchStatusID,
                    DispatchTaskId=t.l.l.DispatchTaskID,
                    AccountId=t.l.l.AccountID,
                    Name=t.l.r.Name,
                    Telephone=t.l.r.Telephone,
                    Stat=t.l.l.Stat,
                    ReasonID=t.l.l.ReasonID,
                    Driver=t.l.l.ContactName,
                    PickupTime=t.l.l.PickupTime,
                    LastWorked=t.l.l.LastWorked,
                    CreatedBy=t.l.l.CreatedBy
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetDispatchTask(int dispatchTaskId)
        {
            var tsk = _wdb.DispatchTasks.Join(_wdb.Accounts, l => l.AccountID, r => r.AccountID, (l, r) => new { l, r }).Join(_wdb.DispatchStatuses, l => l.l.DispatchStatusID, r => r.DispatchStatusID, (l, r) => new { l, r }).FirstOrDefault(t=>t.l.l.DispatchTaskID==dispatchTaskId);
            var dispatchContacts = _wdb.DispatchContacts.Where(t => t.DispatchTaskID == dispatchTaskId).ToList();
            var dispatchStatuses = _wdb.DispatchStatuses.ToList();

            var data = new
            {
                DispatchTaskID=tsk.l.l.DispatchTaskID,
                AccountID=tsk.l.l.AccountID,
                AddressLine1 = tsk.l.l.AddressLine1,
                AddressLine2 = tsk.l.l.AddressLine2,
                City=tsk.l.l.City,
                State=tsk.l.l.State,
                Zip=tsk.l.l.Zip,
                ReasonID=tsk.l.l.ReasonID,
                Driver=tsk.l.l.ContactName,
                PickupTime=tsk.l.l.PickupTime,
                Stat=tsk.l.l.Stat,
                DispatchStatusID=tsk.l.l.DispatchStatusID,
                DispatchStatuses = dispatchStatuses.Select(l => new {
                    Id = l.DispatchStatusID,
                    Name = l.DispatchStatusName
                }).ToList(),
                Notes = tsk.l.l.Notes,
                DispatchContactEmail= dispatchContacts.Select(l=>new { 
                    Id=l.DispatchContactID,
                    Email=l.DispatchContactEmail
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult SaveDispatchTask(int dispatchTaskID,int accountID,string Address1, string Address2, string state, string city,string zip,int? reasonID,string driver, string pickupTime,bool stat,int dispatchStatusID,List<DispatchContact> dispatchContactEmails)
        {
            var dispatchTask = _wdb.DispatchTasks.FirstOrDefault(t => t.DispatchTaskID == dispatchTaskID);

            if (dispatchTask != null)
            {

                var oldContactEmail =_wdb.DispatchContacts.Where(t => t.DispatchTaskID == dispatchTaskID).ToList();

                var newContactEmails = dispatchContactEmails.Where(t => t.DispatchContactID == 0).ToList();
                var cr = oldContactEmail.Join(dispatchContactEmails, l => l.DispatchContactID, r => r.DispatchContactID, (l, r) => l).ToList();
                var delContactEmails = oldContactEmail.Except(cr).ToList();

                _wdb.DispatchContacts.RemoveRange(delContactEmails);
                _wdb.DispatchContacts.AddRange(newContactEmails);

                dispatchTask.AddressLine1 = Address1;
                dispatchTask.AddressLine2 = Address2;
                dispatchTask.State = state;
                dispatchTask.City = city;
                dispatchTask.Zip = zip;
                dispatchTask.ReasonID = reasonID;
                dispatchTask.ContactName = driver;
                dispatchTask.Stat = stat;
                dispatchTask.DispatchStatusID = dispatchStatusID;

                _wdb.SaveChanges();
                return Content(JsonConvert.SerializeObject("OK"));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("Error"));
            }
        }

        public IActionResult AssignmentManagment()
        {
            return View();
        }
        public IActionResult AssignmentManagmentJson(int? status)
        {
            //get parameters
            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            IQueryable<PhlebotomistAssignment> phlebotomistsAssignment = _ph.PhlebotomistAssignments.Include(x => x.Employee);

            if (status.HasValue && status == 2)
                phlebotomistsAssignment = phlebotomistsAssignment.Where(x => x.Confirmed.HasValue ? x.Confirmed.Value == false : false);

            if (status.HasValue && status == 3)
                phlebotomistsAssignment = phlebotomistsAssignment.Where(x => x.Confirmed.HasValue ? x.Confirmed.Value == true : false);

            var recordsTotal = 0;
            var recordsFiltered = 0;

            if (User.IsInRole("Phlebotomy Manager"))
            {
                recordsTotal = phlebotomistsAssignment.Count();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    phlebotomistsAssignment = (from p in _ph.PhlebotomistAssignments.Include(x => x.Employee)
                                               join a2 in _ph.PhlebotomistAssignmentLines on p.PhlebotomistAssignmentID equals a2.PhlebotomistAssignmentID into gj4
                                               from a in gj4.DefaultIfEmpty()
                                               join e in _ph.Phlebotomists on p.EmployeeID equals e.EmployeeID
                                               join acc in _ph.Accounts on a.AccountID equals acc.AccountID into gj
                                               from acct in gj.DefaultIfEmpty()
                                               join grp in _ph.Groups on a.GroupID equals grp.GroupID into gj2
                                               from grp1 in gj2.DefaultIfEmpty()
                                               join grp2 in _ph.Groups on acct.GroupID equals grp2.GroupID into gj3
                                               from grp3 in gj3.DefaultIfEmpty()
                                               where (
                                               (p.EmployeeID.ToLower().Contains(searchValue.ToLower())) || (p.Month == null ? false : p.Month.ToString().Contains(searchValue))
                                               || (a == null ? false : ((a.AccountID == null ? false : a.AccountID.ToString() == (searchValue)) || (a.GroupID == null ? false : a.GroupID.ToString() == (searchValue))))
                                               || ((e.FirstName == null || e.FirstName == "" ? false : e.FirstName.ToUpper().Contains(searchValue.ToUpper())) || (e.LastName == null || e.LastName == "" ? false : e.LastName.ToUpper().Contains(searchValue.ToUpper())) ||                (e.Telephone == null || e.Telephone == "" ? false : e.Telephone.Contains(searchValue)) ||
                                                (e.State == null ? false : e.State.ToUpper().Contains(searchValue.ToUpper())))
                                               || ((acct == null ? false : (acct.Name.ToUpper().Contains(searchValue.ToUpper()) || acct.GroupID.ToString() == (searchValue.ToUpper()))))
                                               || ((grp3 == null ? false : (grp3.SalesRep.ToUpper().Contains(searchValue.ToUpper()) || grp3.Name.ToUpper().Contains(searchValue.ToUpper()))))
                                               || ((grp1 == null ? false : (grp1.SalesRep.ToUpper().Contains(searchValue.ToUpper()) || grp1.Name.ToUpper().Contains(searchValue.ToUpper()))))
                                               )
                                               select p);

                    recordsFiltered = phlebotomistsAssignment.Count();
                }
                else
                {
                    recordsFiltered = recordsTotal;
                }
            }
            else if (User.IsInRole("Phlebotomy User"))
            {
                var Claims = User.Claims.Where(c=>c.Type == "PhlebotomyLead").Select(c=>c.Value).ToList();

                if (!string.IsNullOrEmpty(searchValue))
                {

                    phlebotomistsAssignment = (from p in _ph.PhlebotomistAssignments.Include(x => x.Employee)
                                               join a2 in _ph.PhlebotomistAssignmentLines on p.PhlebotomistAssignmentID equals a2.PhlebotomistAssignmentID into gj4
                                               from a in gj4.DefaultIfEmpty()
                                               join e in _ph.Phlebotomists on p.EmployeeID equals e.EmployeeID
                                               join acc in _ph.Accounts on a.AccountID equals acc.AccountID into gj
                                               from acct in gj.DefaultIfEmpty()
                                               join grp in _ph.Groups on a.GroupID equals grp.GroupID into gj2
                                               from grp1 in gj2.DefaultIfEmpty()
                                               join grp2 in _ph.Groups on acct.GroupID equals grp2.GroupID into gj3
                                               from grp3 in gj3.DefaultIfEmpty()
                                               join pl in _ph.PhlebotomistLeads on p.EmployeeID equals pl.EmployeeID
                                               where
                                               Claims.Contains(pl.LeadID.ToString())
                                               select p
                                               );

                    recordsTotal = phlebotomistsAssignment.Count();


                    phlebotomistsAssignment = (from p in _ph.PhlebotomistAssignments.Include(x => x.Employee)
                                               join a2 in _ph.PhlebotomistAssignmentLines on p.PhlebotomistAssignmentID equals a2.PhlebotomistAssignmentID into gj4
                                               from a in gj4.DefaultIfEmpty()
                                               join e in _ph.Phlebotomists on p.EmployeeID equals e.EmployeeID
                                               join acc in _ph.Accounts on a.AccountID equals acc.AccountID into gj
                                               from acct in gj.DefaultIfEmpty()
                                               join grp in _ph.Groups on a.GroupID equals grp.GroupID into gj2
                                               from grp1 in gj2.DefaultIfEmpty()
                                               join grp2 in _ph.Groups on acct.GroupID equals grp2.GroupID into gj3
                                               from grp3 in gj3.DefaultIfEmpty()
                                               join pl in _ph.PhlebotomistLeads on p.EmployeeID equals pl.EmployeeID
                                               where
                                               Claims.Contains(pl.LeadID.ToString()) 
                                               && (
                                               (p.EmployeeID.ToLower().Contains(searchValue.ToLower())) || (p.Month == null ? false : p.Month.ToString().Contains(searchValue))
                                               || (a == null ? false : ((a.AccountID == null ? false : a.AccountID.ToString() == (searchValue)) || (a.GroupID == null ? false : a.GroupID.ToString() == (searchValue))))
                                               || ((e.FirstName == null || e.FirstName == "" ? false : e.FirstName.ToUpper().Contains(searchValue.ToUpper())) || (e.LastName == null || e.LastName == "" ? false : e.LastName.ToUpper().Contains(searchValue.ToUpper())) || (e.Telephone == null || e.Telephone == "" ? false : e.Telephone.Contains(searchValue)) ||
                                                (e.State == null ? false : e.State.ToUpper().Contains(searchValue.ToUpper())))
                                               || ((acct == null ? false : (acct.Name.ToUpper().Contains(searchValue.ToUpper()) || acct.GroupID.ToString() == (searchValue.ToUpper()))))
                                               || ((grp3 == null ? false : (grp3.SalesRep.ToUpper().Contains(searchValue.ToUpper()) || grp3.Name.ToUpper().Contains(searchValue.ToUpper()))))
                                               || ((grp1 == null ? false : (grp1.SalesRep.ToUpper().Contains(searchValue.ToUpper()) || grp1.Name.ToUpper().Contains(searchValue.ToUpper()))))
                                               )
                                               select p);

                    recordsFiltered =phlebotomistsAssignment.Count();
                } else
                {

                    phlebotomistsAssignment = phlebotomistsAssignment
                                               .Join(_ph.PhlebotomistLeads, l => l.EmployeeID, r => r.EmployeeID, (l, r) => new { l, r }).Where(t => Claims.Contains(t.r.LeadID.ToString())).Select(t => t.l);

                    recordsFiltered = recordsTotal;
                }

            }
            switch (orderColumn)
            {
                case 0:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Confirmed) : phlebotomistsAssignment.OrderByDescending(x => x.Confirmed);
                    break;
                case 1:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.EmployeeID) : phlebotomistsAssignment.OrderByDescending(x => x.Confirmed);
                    break;
                case 2:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Employee.FirstName) : phlebotomistsAssignment.OrderByDescending(x => x.Employee.FirstName);
                    break;
                case 4:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Employee.LastName) : phlebotomistsAssignment.OrderByDescending(x => x.Employee.LastName);
                    break;
                case 5:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Employee.State) : phlebotomistsAssignment.OrderByDescending(x => x.Employee.State);
                    break;
                case 6:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Employee.Telephone) : phlebotomistsAssignment.OrderByDescending(x => x.Employee.Telephone);
                    break;
                case 7:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Month.Month) : phlebotomistsAssignment.OrderByDescending(x => x.Month.Month);
                    break;
                case 8:
                    phlebotomistsAssignment = (orderDirect == "asc") ? phlebotomistsAssignment.OrderBy(x => x.Month.Year) : phlebotomistsAssignment.OrderByDescending(x => x.Month.Year);
                    break;
                default:
                    break;
            }


            var phlebotomistsAssignmentList = phlebotomistsAssignment.Skip(start).Take(length).ToList(); 

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = phlebotomistsAssignmentList.Select(t => new
                {
                    PhlebotomistAssignmentID=t.PhlebotomistAssignmentID,
                    Confirmed=t.Confirmed,
                    EmployeeID=t.EmployeeID,
                    FirstName=t.Employee.FirstName,
                    LastName=t.Employee.LastName,
                    State=t.Employee.State,
                    Telephone=t.Employee.Telephone,
                    Month=t.Month.Month,
                    Year=t.Month.Year
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetPhlebotomistAssignment(int phlebotomistAssignmentID)
        {

            return null;
        }
        private bool FilterData(string find, PhlebotomistAssignment a)
        {
            return (a.EmployeeID.ToLower().Contains(find.ToLower())) || (a.Month == null ? false : a.Month.ToString().Contains(find));
        }
        private bool FilterData(string find, Phlebotomist a)
        {
            return (a.FirstName == null || a.FirstName == "" ? false : a.FirstName.ToUpper().Contains(find.ToUpper())) ||
                (a.LastName == null || a.LastName == "" ? false : a.LastName.ToUpper().Contains(find.ToUpper())) ||
                (a.Telephone == null || a.Telephone == "" ? false : a.Telephone.Contains(find)) ||
                (a.State == null ? false : a.State.ToUpper().Contains(find.ToUpper()));
        }
        private bool FilterData(string find, PhlebotomistAssignmentLine a)
        {
            return a == null ? false : ((a.AccountID == null ? false : a.AccountID.ToString() == (find)) ||
                    (a.GroupID == null ? false : a.GroupID.ToString() == (find)));
        }
        private bool FilterData(string find, Account a)
        {
            return (a == null ? false : (a.Name.ToUpper().Contains(find.ToUpper()) || a.GroupID.ToString() == (find.ToUpper())));
        }
        private bool FilterData(string find, PhlebotomyDB.Group a)
        {
            return (a == null ? false : (a.SalesRep.ToUpper().Contains(find.ToUpper()) || a.Name.ToUpper().Contains(find.ToUpper())));
        }

        public IActionResult GetEmployeeListJson(string searchValue)
        {
            var data = _ph.Phlebotomists.Where(t => t.FirstName.Contains(searchValue) || t.LastName.Contains(searchValue) || t.EmployeeID.Contains(searchValue)).OrderBy(t => t.FirstName).ThenBy(t => t.LastName).Select(t=>new
            {
                EmployeeID=t.EmployeeID,
                FirstName=t.FirstName,
                LastName=t.LastName,
                Telephone=t.Telephone,
                State=t.State
            }).Take(100).ToList();

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetAccountsGroupJson(string type,string searchValue)
        {
            if (type == "account")
            {
                var data = _ph.Accounts.Include(x => x.Group).Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || t.Name.Contains(searchValue)).OrderBy(t => t.Name).Take(10).Select(l => new
                {
                    ID = l.AccountID,
                    Name = l.Name,
                    SalesRep = l.Group.SalesRep

                }).ToList();

                return Content(JsonConvert.SerializeObject(data));
            }
            if (type == "group")
            {
                var data = _ph.Groups.Where(t => Convert.ToString(t.GroupID).Contains(searchValue) || t.Name.Contains(searchValue)).OrderBy(t => t.Name).Take(10).Select(l => new
                {
                    ID = l.GroupID,
                    Name = l.Name,
                    SalesRep = l.SalesRep
                }).ToList();

                return Content(JsonConvert.SerializeObject(data));
            }

            return Content(JsonConvert.SerializeObject("Error"));
        }
        public IActionResult SaveAssignMent(string employeeID,int month,double?[] percent,int[] ids,string[] typs)
        {

            var pa = new PhlebotomistAssignment
            {
                 EmployeeID=employeeID,
                 Month=new DateTime(DateTime.Now.Year,month,1),
                 LastModifiedBy=User.Identity.Name,
                 LastModifiedDate=DateTime.Now
            };

            _ph.PhlebotomistAssignments.Add(pa);

            _ph.SaveChanges();

            for(int i = 0; i < ids.Length; i++)
            {
                _ph.PhlebotomistAssignmentLines.Add(new PhlebotomistAssignmentLine
                {
                    AccountID = typs[i] == "account" ? ids[i] : null,
                    GroupID = typs[i] == "group" ? ids[i] : null,
                    Percentage = percent[i].HasValue ? percent[i].Value : 0,
                     PhlebotomistAssignmentID=pa.PhlebotomistAssignmentID
                });
            }

            _ph.SaveChanges();

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public IActionResult GetPhlebotomyAssignment(int phlebotomistAssignmentID)
        {
            var pa = _ph.PhlebotomistAssignments.Include(x => x.Employee).FirstOrDefault(t => t.PhlebotomistAssignmentID == phlebotomistAssignmentID);
            if (pa != null) {
                var data = new
                {
                    EmployeeID = pa.EmployeeID,
                    FirstName = pa.Employee.FirstName,
                    LastName = pa.Employee.LastName,
                    Telephone = pa.Employee.Telephone,
                    LastModifiedBy = pa.LastModifiedBy,
                    LastModifiedDate = pa.LastModifiedDate,
                    Confirmed = pa.Confirmed,
                    State=pa.Employee.State,
                    Month=pa.Month.Month,
                    Year=pa.Month.Year,
                    PhlebotomyAssignmentLines = _ph.PhlebotomistAssignmentLines.Include(l => l.Account).Include(l => l.Group).Where(l => l.PhlebotomistAssignmentID == phlebotomistAssignmentID)
                    .Select(k => new
                    {
                        PhlebotomyAssignmentID = k.PhlebotomistAssignmentID,
                        PhlebotomistAssignmentLineID = k.PhlebotomistAssignmentLineID,
                        ID = k.AccountID != null ? k.AccountID : (k.GroupID != null ? k.GroupID : null),
                        Name = k.AccountID != null ? k.Account.Name : (k.GroupID != null ? k.Group.Name : null),
                        SalesRep = k.AccountID != null ? k.Account.Group.SalesRep : (k.GroupID != null ? k.Group.SalesRep : null),
                        type = k.AccountID != null ? "account" : (k.GroupID != null ? "group" : null),
                        Percentage=k.Percentage
                    }).ToList()
                };
                return Content(JsonConvert.SerializeObject(data));
            }
            return Content(JsonConvert.SerializeObject("Error"));
        }

        public IActionResult Accession()
        {
            return View("AccessionList");
        }
        public IActionResult AccessionListJson(DateTime? date1,DateTime? date2)
        {
            //get parameters
            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var claim = from c in User.Claims where c.Type == "Account" select c.Value;
            var Claims = claim.ToList();
            var claim2 = (from c in User.Claims where c.Type == "PhlebotomyLead" select c.Value).ToList();

            var Claims1 = _ph.Phlebotomists
                        .Join(_ph.PhlebotomistLeads, l1 => l1.EmployeeID, r1 => r1.EmployeeID, (l2, r2) => new { l2, r2 })
                        .Join(_ph.PhlebotomistAssignments, l3 => l3.l2.EmployeeID, r3 => r3.EmployeeID, (l4, r4) => new { l4, r4 })
                        .Join(_ph.PhlebotomistAssignmentLines, l5 => l5.r4.PhlebotomistAssignmentID, r5 => r5.PhlebotomistAssignmentID, (l6, r6) => new { l6, r6 })
                        .AsEnumerable()
                        .Where(t => claim2.Contains(Convert.ToString(t.l6.l4.r2.LeadID)) && t.r6.AccountID.HasValue && t.l6.r4.Month == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                        .Select(t => Convert.ToString(t.r6.AccountID)).ToList();

            Claims = Claims.Concat(Claims1).ToList();


            var grpclaim = from c in User.Claims where c.Type == "Group" select c.Value;
            var repclaim = (from c in User.Claims where c.Type == "SalesRep" select c.Value).ToList();

            var GrpClaims = grpclaim.Union((from g in _wdb.Groups where repclaim.Contains(g.SalesRep) || repclaim.Contains(g.Manager) select g.GroupID.ToString()).ToList()).ToList();
            var grpclaim2 = (from c in User.Claims where c.Type == "PhlebotomyLead" select c.Value).ToList();

            var GrpClaims1= _ph.Phlebotomists
                        .Join(_ph.PhlebotomistLeads, l1 => l1.EmployeeID, r1 => r1.EmployeeID, (l2, r2) => new { l2, r2 })
                        .Join(_ph.PhlebotomistAssignments, l3 => l3.l2.EmployeeID, r3 => r3.EmployeeID, (l4, r4) => new { l4, r4 })
                        .Join(_ph.PhlebotomistAssignmentLines, l5 => l5.r4.PhlebotomistAssignmentID, r5 => r5.PhlebotomistAssignmentID, (l6, r6) => new { l6, r6 })
                        .AsEnumerable()
                        .Where(t => claim2.Contains(Convert.ToString(t.l6.l4.r2.LeadID)) && t.r6.GroupID.HasValue && t.l6.r4.Month == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                        .Select(t => Convert.ToString(t.r6.GroupID)).ToList();
            GrpClaims = GrpClaims.Concat(GrpClaims1).ToList();

            if (date1 == null)
            {
                date1 = DateTime.Today;
            }

            var Date1 = date1;

            if (date2 == null)
            {
                date2 = DateTime.Today;
            }
            var Date2 = date2;

            date2 = date2.Value.AddDays(1);

            IQueryable<Accession> customAccessions = from a in _wdb.Accessions
                                                     orderby a.Status, a.Created descending
                                                     select a;

            if (User.IsInRole("Phlebotomy Manager"))
            {
                customAccessions = customAccessions.Where(a => a.Created >= date1 && a.Created < date2);
            }
            else
            {
                customAccessions = customAccessions.Join(_wdb.Accounts, l1 => l1.AccountID, r1 => r1.AccountID, (l2, r2) => new { l2, r2 })
                    .Where(t => t.l2.Created >= date1 && t.l2.Created < date2 && (t.l2.CreatedBy == User.Identity.Name || Claims.Contains(t.l2.AccountID.ToString()) || GrpClaims.Contains(t.r2.GroupID.ToString())))
                    .Select(t => t.l2);
            }

            var recordsTotal = customAccessions.Count();
            var recordsFiltered = recordsTotal;

            switch (orderColumn)
            {
                case 0:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.Status) : customAccessions.OrderByDescending(x => x.Status);
                    break;
                case 1:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.AccountID) : customAccessions.OrderByDescending(x => x.AccountID);
                    break;
                case 2:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.PatientName) : customAccessions.OrderByDescending(x => x.PatientName); 
                    break;
                case 3:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.PatientDOB) : customAccessions.OrderByDescending(x => x.PatientDOB);
                    break;
                case 5:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.InsuranceName) : customAccessions.OrderByDescending(x => x.InsuranceName);
                    break;
                case 6:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.InsuranceID) : customAccessions.OrderByDescending(x => x.InsuranceID);
                    break;
                case 12:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.LastModified) : customAccessions.OrderByDescending(x => x.LastModified);
                    break;
                case 13:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.CreatedBy) : customAccessions.OrderByDescending(x => x.CreatedBy);
                    break;
                default:
                    customAccessions = orderDirect == "asc" ? customAccessions.OrderBy(x => x.AccountID) : customAccessions.OrderByDescending(x => x.AccountID);
                    break;
            }

            var accession = customAccessions.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = accession.Select(t => new
                {
                    AccessionId=t.AccessionID,
                    Status=t.Status,
                    AccountId = t.AccountID,
                    PatientName = t.PatientName,
                    PatientDOB = t.PatientDOB,
                    PatientGender = t.PatientGender,
                    InsuranceName = t.InsuranceName,
                    InsuranceId = t.InsuranceID,
                    Note = t.Note,
                    t.Stat1,
                    t.Stat2,
                    t.Stat3,
                    t.Stat4,
                    LastModifiedBy=t.ModifiedBy.Substring(0,t.ModifiedBy.Length-18),
                    CreatedBy = t.CreatedBy.Substring(0, t.CreatedBy.Length - 18)
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetAccession(int? accessionId)
        {
            var accession = _wdb.Accessions.FirstOrDefault(t => t.AccessionID == accessionId);

            var data = new
            {
                AccessionId= accession.AccessionID,
                AccountId= accession.AccountID,
                InsuranceId= accession.InsuranceID,
                InsuranceName= accession.InsuranceName,
                Note= accession.Note,
                PatientDOB= accession.PatientDOB,
                PatientName= accession.PatientName,
                PatientGender= accession.PatientGender
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult SaveAcession(int? accessionId,int? accountId,string patientName,DateTime? patientDOB,string insuranceName,string insuranceId,string note,bool? stat1,bool? stat2,bool? stat3,bool? stat4)
        {
            var accession = _wdb.Accessions.FirstOrDefault(t => t.AccessionID == accessionId);
            if (accession != null)
            {
                accession.AccountID = accountId.Value;
                accession.PatientName = patientName;
                accession.PatientDOB = patientDOB;
                accession.InsuranceName = insuranceName;
                accession.InsuranceID = insuranceId;
                accession.Note = note??string.Empty;
                accession.Stat1 = stat1.Value;
                accession.Stat2 = stat2.Value;
                accession.Stat3 = stat3.Value;
                accession.Stat4 = stat4.Value;
            }
            else
            {
                accession = new Accession
                {
                     PatientDOB=patientDOB,
                     ModifiedBy=User.Identity.Name,
                     LastModified=DateTime.Now,
                     AccountID=accountId.Value,
                     Created=DateTime.Now,
                     CreatedBy=User.Identity.Name,
                     InsuranceID=insuranceId,
                     InsuranceName=insuranceName,
                     Note=note??string.Empty,
                     PatientName=patientName,
                     Stat1 = stat1.Value,
                     Stat2 = stat2.Value,
                     Stat3 = stat3.Value,
                     Stat4 = stat4.Value
                };
                _wdb.Accessions.Add(accession);
            }

            _wdb.SaveChanges();

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public IActionResult DeletePhlebotomyAssignment(int phlebotomistAssignmentID)
        {

            var items = _ph.PhlebotomistAssignmentLines.Where(t => t.PhlebotomistAssignmentID == phlebotomistAssignmentID).ToList();
            var item = _ph.PhlebotomistAssignments.FirstOrDefault(t => t.PhlebotomistAssignmentID == phlebotomistAssignmentID);

            if (item != null)
            {
                _ph.PhlebotomistAssignmentLines.RemoveRange(items);
                _ph.PhlebotomistAssignments.Remove(item);
                _ph.SaveChanges();
            }

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public IActionResult AccountDictionary()
        {
            return View();
        }
        public IActionResult ActionDictionaryJson(bool? Status)
        {
            //get parameters
            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var Search = Request.Form["search[value]"].ToString();

            var claims = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var RepClaims = claims.ToList();
            claims = from c in User.Claims where c.Type == "Account" select c.Value;
            var AccClaims = claims.ToList();
            claims = from c in User.Claims where c.Type == "Group" select c.Value;
            var GrpClaims = claims.ToList();



            IQueryable<Account> accounts = _ph.Accounts;

            if (Status.HasValue) //Active
            {
                accounts = accounts.Where(t => t.Active == Status.Value);
            }

            var recordsTotal = accounts.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(Search))
            {
                accounts = accounts.Where(t => Convert.ToString(t.AccountID) == Search && t.Name.Contains(Search) || t.Address.Contains(Search));

                var acc2 = from a in accounts
                           join l in _ph.PhlebotomistAssignmentLines on a.AccountID equals l.AccountID
                           join p in _ph.PhlebotomistAssignments on l.PhlebotomistAssignmentID equals p.PhlebotomistAssignmentID
                           join e in _ph.Phlebotomists on p.EmployeeID equals e.EmployeeID
                           where (e.FirstName + " " + e.LastName).Contains(Search) && p.Month == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                           select a;

                accounts = accounts.Union(acc2);
            }

            recordsFiltered = accounts.Count();

            switch (orderColumn)
            {
                case 0:
                    accounts = orderDirect=="asc"?accounts.OrderBy(x => x.AccountID): accounts.OrderByDescending(x => x.AccountID);
                    break;
                case 1:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.Name) : accounts.OrderByDescending(x => x.Name);
                    break;
                case 2:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.Telephone) : accounts.OrderByDescending(x => x.Telephone);
                    break;
                case 3:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.Fax) : accounts.OrderByDescending(x => x.Fax);
                    break;
                case 4:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.Address) : accounts.OrderByDescending(x => x.Address);
                    break;
                case 5:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.City) : accounts.OrderByDescending(x => x.City);
                    break;
                case 6:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.State) : accounts.OrderByDescending(x => x.State);
                    break;
                default:
                    accounts = orderDirect == "asc" ? accounts.OrderBy(x => x.AccountID) : accounts.OrderByDescending(x => x.AccountID);
                    break;
            }

            // assign the current page of items to the Items property
            var accountsData = accounts.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = accountsData.Select(t => new
                {
                    AccountId=t.AccountID,
                    Active=t.Active, 
                    Address=t.Address,
                    City=t.City,
                    Telephone=t.Telephone,
                    Fax=t.Fax,
                    Name=t.Name,
                    State=t.State
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
    }
}
