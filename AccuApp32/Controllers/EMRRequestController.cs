using AccuApp32MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhlebotomyDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DinkToPdf;
using AccuApp.Services;
using DinkToPdf.Contracts;
using AccuApp32MVC.Extensions;

namespace AccuApp32MVC.Controllers
{

    public class EMRRequestController : Controller
    {
        private readonly WebdbContext _wdb;
        private readonly WebdbContextProcedures _contextProcedureWebDb;
        private readonly PhlebotomyContext _ph;
        private readonly UserManager<IdentityUser> _userManager;


        private readonly IEmailSender _emailSender;
        private readonly ITemplateService _templateService;
        private readonly IConverter _pdfConverter;

        public List<String> Statuses = new List<String>() { "In process", "Testing", "Denied", "Canceled", "Live", "Disconnected", "On Hold", "New", "Pending Quote Approval", "Quote Approved", "Quote Requested", "Acknowledged" };

        public EMRRequestController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext contextPhlebotomy, IEmailSender emailSender,
            ITemplateService templateService, IConverter pdfConverter, UserManager<IdentityUser> userManager)
        {
            _wdb = contextWebDb;
            _contextProcedureWebDb = contextProcedureWebDb;
            _ph = contextPhlebotomy;
            _templateService = templateService;
            _pdfConverter = pdfConverter;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var accounts = from a in _wdb.Accounts where ((Claims.Contains(a.Group.SalesRep)) || (Claims.Contains(a.Group.Manager)) || (Groups.Contains(a.Group.GroupID.ToString()))) select a;
            var Accounts = accounts.ToList();
            var vendors = from a in _wdb.Vendors select a;
            var Vendor = vendors.ToList();

            Dictionary<int, string> acc = new Dictionary<int, string>();

            foreach (var a in accounts)
            {
                acc.Add(a.AccountID, Convert.ToString(a.AccountID) + " - " + Convert.ToString(a.Name));
            }

            Dictionary<int, string> vd = new Dictionary<int, string>();

            foreach (var v in Vendor)
            {
                vd.Add(v.VendorID, v.Name);
            }


            ViewBag.AccountList = acc;
            ViewBag.VendorList = vd;

            return View();
        }
        public IActionResult NewEmrRequests()
        {

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var accounts = from a in _wdb.Accounts where ((Claims.Contains(a.Group.SalesRep)) || (Claims.Contains(a.Group.Manager)) || (Groups.Contains(a.Group.GroupID.ToString()))) select a;
            var Accounts = accounts.ToList();
            var vendors = from a in _wdb.Vendors select a;
            var Vendor = vendors.OrderBy(t => t.Name).ToList();

            Dictionary<int, string> acc = new Dictionary<int, string>();

            foreach (var a in accounts)
            {
                acc.Add(a.AccountID, Convert.ToString(a.AccountID) + " - " + Convert.ToString(a.Name));
            }

            Dictionary<int, string> vd = new Dictionary<int, string>();

            foreach (var v in Vendor)
            {
                vd.Add(v.VendorID, v.Name);
            }


            ViewBag.AccountList = acc;
            ViewBag.VendorList = vd;

            return View("newemrrequests");
        }
        public IActionResult NewEmrRequests1()
        {

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var accounts = from a in _wdb.Accounts where ((Claims.Contains(a.Group.SalesRep)) || (Claims.Contains(a.Group.Manager)) || (Groups.Contains(a.Group.GroupID.ToString()))) select a;
            var Accounts = accounts.ToList();
            var vendors = from a in _wdb.Vendors select a;
            var Vendor = vendors.OrderBy(t => t.Name).ToList();

            Dictionary<int, string> acc = new Dictionary<int, string>();

            foreach (var a in accounts)
            {
                acc.Add(a.AccountID, Convert.ToString(a.AccountID) + " - " + Convert.ToString(a.Name));
            }

            Dictionary<int, string> vd = new Dictionary<int, string>();

            foreach (var v in Vendor)
            {
                vd.Add(v.VendorID, v.Name);
            }


            ViewBag.AccountList = acc;
            ViewBag.VendorList = vd;

            return View("newemrrequests1");
        }
        public IActionResult CompletedEmrRequests()
        {

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var accounts = from a in _wdb.Accounts where ((Claims.Contains(a.Group.SalesRep)) || (Claims.Contains(a.Group.Manager)) || (Groups.Contains(a.Group.GroupID.ToString()))) select a;
            var Accounts = accounts.ToList();
            var vendors = from a in _wdb.Vendors select a;
            var Vendor = vendors.OrderBy(t => t.Name).ToList();

            Dictionary<int, string> acc = new Dictionary<int, string>();

            foreach (var a in accounts)
            {
                acc.Add(a.AccountID, Convert.ToString(a.AccountID) + " - " + Convert.ToString(a.Name));
            }

            Dictionary<int, string> vd = new Dictionary<int, string>();

            foreach (var v in Vendor)
            {
                vd.Add(v.VendorID, v.Name);
            }


            ViewBag.AccountList = acc;
            ViewBag.VendorList = vd;

            return View("completedemrrequests");
        }
        public IActionResult NewEMRRequestListJson(int[] requestStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();

            var emrReq = _wdb.EmrRequests.GroupJoin(_wdb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .GroupJoin(_wdb.Accounts, l5 => l5.l4.l2.AccountID, r5 => r5.AccountID, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                .GroupJoin(_wdb.Groups, l9 => l9.r8.GroupID, r9 => r9.GroupID, (l10, r10) => new { l10, r10 })
                .SelectMany(l11 => l11.r10.DefaultIfEmpty(), (l12, r12) => new { l12, r12 })
                .Select(t => new
                {
                    t.l12.l10.l8.l6.l4.l2.EMRRequestID,
                    t.l12.l10.l8.l6.l4.l2.RequestedDate,
                    t.l12.l10.l8.l6.l4.l2.AccountID,
                    Status = t.l12.l10.l8.l6.l4.l2.RequestStatus,
                    AccountName = t.l12.l10.r8.Name,
                    VendorID = t.l12.l10.l8.l6.r4.VendorID,
                    VendorName = t.l12.l10.l8.l6.r4.Name,
                    t.l12.l10.l8.l6.l4.l2.ConnectionType,
                    t.l12.l10.l8.l6.l4.l2.RequestedBy,
                    t.r12.SalesRep,
                    t.r12.Manager,
                    t.l12.l10.r8.GroupID
                })
                .Where(t => ((Claims.Contains(t.SalesRep)) || (Claims.Contains(t.Manager)) || (Groups.Contains(t.GroupID.ToString()))));

            if (requestStatus.Length > 0)
                emrReq = emrReq.Where(t => requestStatus.Contains(t.Status));
            else
            {
                emrReq = emrReq.Where(a => (a.Status == 0 || a.Status == 1 || a.Status == 6 || a.Status == 7 || a.Status == 8 || a.Status == 9 || a.Status == 10 || a.Status == 11));
            }

            var emrRequests = emrReq.Select(t => new {
                t.EMRRequestID,
                t.RequestedDate,
                t.AccountID,
                t.Status,
                t.AccountName,
                t.VendorID,
                t.VendorName,
                t.ConnectionType,
                t.RequestedBy,
                t.SalesRep,
                t.Manager,
                t.GroupID
            });

            var recordsTotal = emrRequests.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                emrRequests = emrRequests.Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || t.AccountName.Contains(searchValue));
                recordsFiltered = emrRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedDate) : emrRequests.OrderByDescending(t => t.RequestedDate);
            }
            if (orderColumn == 1)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.Status) : emrRequests.OrderByDescending(t => t.Status);
            }
            if (orderColumn == 2)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountID) : emrRequests.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 3)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountName) : emrRequests.OrderByDescending(t => t.AccountName);
            }
            if (orderColumn == 4)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.VendorName) : emrRequests.OrderByDescending(t => t.VendorName);
            }
            if (orderColumn == 5)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.ConnectionType) : emrRequests.OrderByDescending(t => t.ConnectionType);
            }
            if (orderColumn == 6)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedBy) : emrRequests.OrderByDescending(t => t.RequestedBy);
            }

            var emrRequestsList = emrRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = emrRequestsList.Select(t => new {
                    t.EMRRequestID,
                    t.AccountID,
                    t.AccountName,
                    t.VendorID,
                    t.VendorName,
                    t.Status,
                    StatusName = Statuses[t.Status],
                    t.ConnectionType,
                    t.RequestedBy,
                    t.RequestedDate
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult NewEMRRequestList1Json(int[] requestStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var emrReq = _wdb.EmrRequests.GroupJoin(_wdb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .GroupJoin(_wdb.Accounts, l5 => l5.l4.l2.AccountID, r5 => r5.AccountID, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                .Select(t => new
                {
                    t.l8.l6.l4.l2.EMRRequestID,
                    t.l8.l6.l4.l2.RequestedDate,
                    t.l8.l6.l4.l2.AccountID,
                    Status = t.l8.l6.l4.l2.RequestStatus,
                    AccountName = t.r8.Name,
                    VendorID = t.l8.l6.r4.VendorID,
                    VendorName = t.l8.l6.r4.Name,
                    t.l8.l6.l4.l2.ConnectionType,
                    t.l8.l6.l4.l2.RequestedBy
                })
                .Where(a => (a.Status == 0 || a.Status == 1 || a.Status == 6 || a.Status == 7 || a.Status == 8 || a.Status == 9 || a.Status == 10 || a.Status == 11));

            if (requestStatus.Length > 0)
                emrReq = emrReq.Where(t => requestStatus.Contains(t.Status));

            var emrRequests = emrReq.Select(t => new {
                t.EMRRequestID,
                t.RequestedDate,
                t.AccountID,
                t.Status,
                t.AccountName,
                t.VendorID,
                t.VendorName,
                t.ConnectionType,
                t.RequestedBy
            });

            var recordsTotal = emrRequests.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                emrRequests = emrRequests.Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || t.AccountName.Contains(searchValue));
                recordsFiltered = emrRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedDate) : emrRequests.OrderByDescending(t => t.RequestedDate);
            }
            if (orderColumn == 1)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.Status) : emrRequests.OrderByDescending(t => t.Status);
            }
            if (orderColumn == 2)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountID) : emrRequests.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 3)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountName) : emrRequests.OrderByDescending(t => t.AccountName);
            }
            if (orderColumn == 4)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.VendorName) : emrRequests.OrderByDescending(t => t.VendorName);
            }
            if (orderColumn == 5)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.ConnectionType) : emrRequests.OrderByDescending(t => t.ConnectionType);
            }
            if (orderColumn == 6)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedBy) : emrRequests.OrderByDescending(t => t.RequestedBy);
            }

            var emrRequestsList = emrRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = emrRequestsList.Select(t => new {
                    t.EMRRequestID,
                    t.AccountID,
                    t.AccountName,
                    t.VendorID,
                    t.VendorName,
                    t.Status,
                    StatusName = Statuses[t.Status],
                    t.ConnectionType,
                    t.RequestedBy,
                    t.RequestedDate
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult CompletedEMRRequestListJson(int[] requestStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();

            var emrReq = _wdb.EmrRequests.GroupJoin(_wdb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .GroupJoin(_wdb.Accounts, l5 => l5.l4.l2.AccountID, r5 => r5.AccountID, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                .GroupJoin(_wdb.Groups, l9 => l9.r8.GroupID, r9 => r9.GroupID, (l10, r10) => new { l10, r10 })
                .SelectMany(l11 => l11.r10.DefaultIfEmpty(), (l12, r12) => new { l12, r12 })
                .Select(t => new
                {
                    t.l12.l10.l8.l6.l4.l2.EMRRequestID,
                    t.l12.l10.l8.l6.l4.l2.RequestedDate,
                    t.l12.l10.l8.l6.l4.l2.AccountID,
                    Status = t.l12.l10.l8.l6.l4.l2.RequestStatus,
                    AccountName = t.l12.l10.r8.Name,
                    VendorID = t.l12.l10.l8.l6.r4.VendorID,
                    VendorName = t.l12.l10.l8.l6.r4.Name,
                    t.l12.l10.l8.l6.l4.l2.ConnectionType,
                    t.l12.l10.l8.l6.l4.l2.RequestedBy,
                    t.r12.SalesRep,
                    t.r12.Manager,
                    t.l12.l10.r8.GroupID
                })
                .Where(t => ((Claims.Contains(t.SalesRep)) || (Claims.Contains(t.Manager)) || (Groups.Contains(t.GroupID.ToString()))));

            if (requestStatus.Length > 0)
                emrReq = emrReq.Where(t => requestStatus.Contains(t.Status));
            else
            {
                emrReq = emrReq.Where(a => (a.Status != 0 && a.Status != 1 && a.Status != 6 && a.Status != 7 && a.Status != 8 && a.Status != 9 && a.Status != 10 && a.Status != 11));
            }

            var emrRequests = emrReq.Select(t => new {
                t.EMRRequestID,
                t.RequestedDate,
                t.AccountID,
                t.Status,
                t.AccountName,
                t.VendorID,
                t.VendorName,
                t.ConnectionType,
                t.RequestedBy,
                t.SalesRep,
                t.Manager,
                t.GroupID
            });

            var recordsTotal = emrRequests.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                emrRequests = emrRequests.Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || t.AccountName.Contains(searchValue));
                recordsFiltered = emrRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedDate) : emrRequests.OrderByDescending(t => t.RequestedDate);
            }
            if (orderColumn == 1)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.Status) : emrRequests.OrderByDescending(t => t.Status);
            }
            if (orderColumn == 2)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountID) : emrRequests.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 3)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountName) : emrRequests.OrderByDescending(t => t.AccountName);
            }
            if (orderColumn == 4)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.VendorName) : emrRequests.OrderByDescending(t => t.VendorName);
            }
            if (orderColumn == 5)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.ConnectionType) : emrRequests.OrderByDescending(t => t.ConnectionType);
            }
            if (orderColumn == 6)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedBy) : emrRequests.OrderByDescending(t => t.RequestedBy);
            }

            var emrRequestsList = emrRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = emrRequestsList.Select(t => new {
                    t.EMRRequestID,
                    t.AccountID,
                    t.AccountName,
                    t.VendorID,
                    t.VendorName,
                    t.Status,
                    StatusName = Statuses[t.Status],
                    t.ConnectionType,
                    t.RequestedBy,
                    t.RequestedDate
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult EMRRequestListJson(int[] requestStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();

            var emrReq = _wdb.EmrRequests.GroupJoin(_wdb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .GroupJoin(_wdb.Accounts, l5 => l5.l4.l2.AccountID, r5 => r5.AccountID, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                .GroupJoin(_wdb.Groups, l9 => l9.r8.GroupID, r9 => r9.GroupID, (l10, r10) => new { l10, r10 })
                .SelectMany(l11 => l11.r10.DefaultIfEmpty(), (l12, r12) => new { l12, r12 })
                .Select(t => new
                {
                    t.l12.l10.l8.l6.l4.l2.EMRRequestID,
                    t.l12.l10.l8.l6.l4.l2.RequestedDate,
                    t.l12.l10.l8.l6.l4.l2.AccountID,
                    Status = t.l12.l10.l8.l6.l4.l2.RequestStatus,
                    AccountName = t.l12.l10.r8.Name,
                    VendorID = t.l12.l10.l8.l6.r4.VendorID,
                    VendorName = t.l12.l10.l8.l6.r4.Name,
                    t.l12.l10.l8.l6.l4.l2.ConnectionType,
                    t.l12.l10.l8.l6.l4.l2.RequestedBy,
                    t.r12.SalesRep,
                    t.r12.Manager,
                    t.l12.l10.r8.GroupID
                })
                .Where(t => ((Claims.Contains(t.SalesRep)) || (Claims.Contains(t.Manager)) || (Groups.Contains(t.GroupID.ToString()))));

            if (requestStatus.Length > 0)
                emrReq = emrReq.Where(t => requestStatus.Contains(t.Status));


            var emrRequests = emrReq.Select(t => new {
                t.EMRRequestID,
                t.RequestedDate,
                t.AccountID,
                t.Status,
                t.AccountName,
                t.VendorID,
                t.VendorName,
                t.ConnectionType,
                t.RequestedBy,
                t.SalesRep,
                t.Manager,
                t.GroupID
            });

            var recordsTotal = emrRequests.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                emrRequests = emrRequests.Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || t.AccountName.Contains(searchValue));
                recordsFiltered = emrRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedDate) : emrRequests.OrderByDescending(t => t.RequestedDate);
            }
            if (orderColumn == 1)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.Status) : emrRequests.OrderByDescending(t => t.Status);
            }
            if (orderColumn == 2)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountID) : emrRequests.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 3)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.AccountName) : emrRequests.OrderByDescending(t => t.AccountName);
            }
            if (orderColumn == 4)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.VendorName) : emrRequests.OrderByDescending(t => t.VendorName);
            }
            if (orderColumn == 5)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.ConnectionType) : emrRequests.OrderByDescending(t => t.ConnectionType);
            }
            if (orderColumn == 6)
            {
                emrRequests = (orderDirect == "asc") ? emrRequests.OrderBy(t => t.RequestedBy) : emrRequests.OrderByDescending(t => t.RequestedBy);
            }

            var emrRequestsList = emrRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = emrRequestsList.Select(t => new {
                    t.EMRRequestID,
                    t.AccountID,
                    t.AccountName,
                    t.VendorID,
                    t.VendorName,
                    t.Status,
                    StatusName = Statuses[t.Status],
                    t.ConnectionType,
                    t.RequestedBy,
                    t.RequestedDate
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetEMRRequest(int? emrRequestId)
        {
            var emrRequest = _wdb.EmrRequests.GroupJoin(_wdb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .GroupJoin(_wdb.Accounts, l5 => l5.l4.l2.AccountID, r5 => r5.AccountID, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                .Join(_wdb.Groups, l9 => l9.r8.GroupID, r9 => r9.GroupID, (l10, r10) => new { l10, r10 })
                .Select(t => new
                {
                    t.l10.l8.l6.l4.l2.EMRRequestID,
                    t.l10.l8.l6.l4.l2.RequestedDate,
                    t.l10.l8.l6.l4.l2.AccountID,
                    Status = t.l10.l8.l6.l4.l2.RequestStatus,
                    AccountName = t.l10.r8.Name,
                    VendorID = t.l10.l8.l6.r4.VendorID,
                    VendorName = t.l10.l8.l6.r4.Name,
                    t.l10.l8.l6.l4.l2.ConnectionType,
                    t.l10.l8.l6.l4.l2.RequestedBy,
                    t.l10.l8.l6.l4.l2.OfficeManagerEmail,
                    t.l10.l8.l6.l4.l2.OfficeManagerName,
                    t.l10.l8.l6.l4.l2.OfficeManagerPhone,
                    t.l10.l8.l6.l4.l2.PhysicianName,
                    t.l10.l8.l6.l4.l2.PhysicianSpecialty,
                    t.l10.l8.l6.l4.l2.ExpectedSpecimenCount,
                    t.l10.l8.l6.l4.l2.HasCustomPanels,
                    t.l10.l8.l6.l4.l2.CustomPanels,
                    t.l10.l8.l6.l4.l2.VendorContact,
                    t.l10.l8.l6.l4.l2.VendorPhone,
                    t.l10.l8.l6.l4.l2.VendorFax,
                    t.l10.l8.l6.l4.l2.VendorEmail,
                    t.l10.l8.l6.l4.l2.VendorInstallationCost,
                    t.l10.l8.l6.l4.l2.VendorMaintenanceCost,
                    t.l10.l8.l6.l4.l2.ProjectedLiveDate,
                    t.l10.l8.l6.l4.l2.ActualLiveDate,
                    t.l10.l8.l6.l4.l2.ComtronTerminationDate,
                    t.l10.l8.l6.l4.l2.VendorTerminationDate,
                    t.l10.l8.l6.l4.l2.LISRequestSent,
                    t.l10.l8.l6.l4.l2.POSent

                }).FirstOrDefault(t => t.EMRRequestID == emrRequestId);

            if (emrRequest != null)
            {

                return Content(JsonConvert.SerializeObject(emrRequest));
            }
            return Content(JsonConvert.SerializeObject("Error"));
        }
        public async Task<IActionResult> NewSaveEMRRequest
            (
                int? EmrRequestID,
                int Status,
                int AccountID,
                int VendorID,
                string OfficeManagerName,
                string OfficeManagerEmail,
                string OfficeManagerPhone,
                string PhysicianName,
                string PhysicianSpecialty,
                int ExpectedSpecimenCount,
                bool HasCustomPanels,
                string CustomPanels,
                string VendorContact,
                string VendorPhone,
                string VendorFax,
                string VendorEmail,
                string ConnectionType
            )
        {

            var emrRequestDb = _wdb.EmrRequests.FirstOrDefault(t =>EmrRequestID.HasValue && t.EMRRequestID == EmrRequestID.Value);

            var vendor = _wdb.Vendors.FirstOrDefault(t => t.VendorID == VendorID);

            string ActionTaken = string.Empty;

            if (emrRequestDb!=null) {

                ActionTaken = emrRequestDb.RequestStatus != Status ? ": An action has been taken" : "";

                emrRequestDb.VendorID = VendorID;
                emrRequestDb.AccountID = AccountID;
                emrRequestDb.ConnectionType = ConnectionType;
                emrRequestDb.CustomPanels = CustomPanels;
                emrRequestDb.ExpectedSpecimenCount = ExpectedSpecimenCount;
                emrRequestDb.HasCustomPanels = HasCustomPanels;
                emrRequestDb.OfficeManagerEmail = OfficeManagerEmail;
                emrRequestDb.OfficeManagerName = OfficeManagerName;
                emrRequestDb.OfficeManagerPhone = OfficeManagerPhone;
                emrRequestDb.PhysicianName = PhysicianName;
                emrRequestDb.PhysicianSpecialty = PhysicianSpecialty;
                emrRequestDb.RequestStatus = Status;
                emrRequestDb.VendorContact = VendorContact;
                emrRequestDb.VendorEmail = VendorEmail;
                emrRequestDb.VendorFax = VendorFax;
                emrRequestDb.VendorPhone = VendorPhone;

                emrRequestDb.ComtronInstallationCost = vendor != null ? (vendor.ComtronInstallationCost == null ? 1100 : vendor.ComtronInstallationCost) : null;

                emrRequestDb.VendorAnnualCost = vendor.VendorAnnualCost;
                emrRequestDb.VendorInstallationCost = vendor.VendorInstallationCost;
                emrRequestDb.VendorMaintenanceCost = vendor.VendorMaintenanceCost;

            }
            else
            {

                emrRequestDb = new EmrRequest
                {
                    VendorID = VendorID,
                    AccountID = AccountID,
                    ConnectionType = ConnectionType,
                    CustomPanels = CustomPanels,
                    ExpectedSpecimenCount = ExpectedSpecimenCount,
                    HasCustomPanels = HasCustomPanels,
                    OfficeManagerEmail = OfficeManagerEmail,
                    OfficeManagerName = OfficeManagerName,
                    OfficeManagerPhone = OfficeManagerPhone,
                    PhysicianName = PhysicianName,
                    PhysicianSpecialty = PhysicianSpecialty,
                    VendorContact = VendorContact,
                    VendorEmail = VendorEmail,
                    VendorFax = VendorFax,
                    VendorPhone = VendorPhone,
                    ComtronInstallationCost = vendor != null ? (vendor.ComtronInstallationCost == null ? 1100 : vendor.ComtronInstallationCost) : null,
                    VendorAnnualCost = vendor.VendorAnnualCost,
                    VendorInstallationCost = vendor.VendorInstallationCost,
                    VendorMaintenanceCost = vendor.VendorMaintenanceCost,

                    RequestedBy = User.Identity.Name,
                    RequestedDate = DateTime.Now,
                    RequestStatus = 7
                };

                _wdb.EmrRequests.Add(emrRequestDb);
            }

            try
            {
                _wdb.SaveChanges();
            }catch(Exception ex)
            {

            }

            string FileData = await GeneratePDFAsync(emrRequestDb.EMRRequestID);

            //create body string for email here

            StringBuilder sb = new StringBuilder();

            if (emrRequestDb != null)
            {

                sb.Append("<htm><body><p>");
                sb.Append($"EMR Request<br />");
                sb.Append($"RequestId: {emrRequestDb.EMRRequestID} <br />");
                sb.Append($"Account: {emrRequestDb.Account.AccountID} <br />");
                sb.Append($"Request Status: {Statuses[emrRequestDb.RequestStatus].ToString()} <br />");
                sb.Append($"Office Manager Name: {emrRequestDb.OfficeManagerName} <br />");
                sb.Append($"Office Phone: {emrRequestDb.OfficeManagerPhone} <br />");
                sb.Append($"Physician Name: {emrRequestDb.PhysicianName} <br />");
                sb.Append($"Physician Specialty: {emrRequestDb.PhysicianSpecialty} <br />");
                sb.Append($"Expected Specimen Count: {emrRequestDb.ExpectedSpecimenCount} <br />");
                if (emrRequestDb.HasCustomPanels)
                {
                    sb.Append($"Custom Panels: {emrRequestDb.CustomPanels} <br />");
                }
                sb.Append($"Vendor Name: {(vendor != null ? vendor.Name : "")} <br />");
                sb.Append($"Vendor Contact: {emrRequestDb.VendorContact} <br />");
                sb.Append($"Vendor Phone: {emrRequestDb.VendorPhone} <br />");
                sb.Append($"Vendor Fax: {emrRequestDb.VendorFax} <br />");
                sb.Append($"Vendor Email: {emrRequestDb.VendorEmail} <br />");
                sb.Append($"Vendor ConnectionType: {emrRequestDb.ConnectionType} <br />");
                sb.Append($"Requested By: {emrRequestDb.RequestedBy} <br />");
                sb.Append($"Requested Date: {emrRequestDb.RequestedDate} <br />");
                sb.Append("</p></body></htm>");

                ActionTaken = ActionTaken = ":Status Changed To " + Statuses[emrRequestDb.RequestStatus].ToString();

                //await _emailSender.SendEmrRequestModifiedNotification(EmrRequest.RequestedBy);
                //await _emailSender.SendEmrRequestModifiedNotification(sb.ToString(), (!string.IsNullOrEmpty(ActionTaken) ?  ActionTaken : ""), EmrRequest.RequestedBy, User.Identity.Name, "", "");

            }

            return Content(JsonConvert.SerializeObject("OK"));
        }

        public async Task<IActionResult> SaveEMRRequest
            (
                int? EmrRequestID,
                int Status,
                int AccountID,
                int VendorID,
                string OfficeManagerName,
                string OfficeManagerEmail,
                string OfficeManagerPhone,
                string PhysicianName,
                string PhysicianSpecialty,
                int ExpectedSpecimenCount,
                bool HasCustomPanels,
                string CustomPanels,
                string VendorContact,
                string VendorPhone,
                string VendorFax,
                string VendorEmail,
                string ConnectionType,
                string ActionTaken,
                DateTime? ProjectedLiveDate,
                DateTime? ActualLiveDate,
                DateTime? ComtronTerminationDate,
                DateTime? VendorTerminationDate,
                bool? Approve,
                string DenialReason
            )
        {

            var emrRequestDb = _wdb.EmrRequests.FirstOrDefault(t => EmrRequestID.HasValue && t.EMRRequestID == EmrRequestID.Value);

            if (emrRequestDb == null)
                emrRequestDb = new EmrRequest();

            emrRequestDb.AccountID = AccountID;
            emrRequestDb.ConnectionType = ConnectionType;
            emrRequestDb.CustomPanels = CustomPanels;
            emrRequestDb.ExpectedSpecimenCount = ExpectedSpecimenCount;
            emrRequestDb.HasCustomPanels = HasCustomPanels;
            emrRequestDb.OfficeManagerEmail = OfficeManagerEmail;
            emrRequestDb.OfficeManagerName = OfficeManagerName;
            emrRequestDb.OfficeManagerPhone = OfficeManagerPhone;
            emrRequestDb.PhysicianName = PhysicianName;
            emrRequestDb.PhysicianSpecialty = PhysicianSpecialty;
            emrRequestDb.VendorContact = VendorContact;
            emrRequestDb.VendorEmail = VendorEmail;
            emrRequestDb.VendorFax = VendorFax;
            emrRequestDb.VendorID = VendorID;
            emrRequestDb.VendorPhone = VendorPhone;
            emrRequestDb.RequestStatus = Status;
            emrRequestDb.ProjectedLiveDate = ProjectedLiveDate;
            emrRequestDb.ActualLiveDate = ActualLiveDate;
            emrRequestDb.ComtronTerminationDate = ComtronTerminationDate;
            emrRequestDb.VendorTerminationDate = VendorTerminationDate;

            _wdb.SaveChanges();

            StringBuilder sb = new StringBuilder();

            if (Approve.HasValue && Approve.Value)
            {

                emrRequestDb.QuoteApprovedDate = DateTime.Now;
                emrRequestDb.QuoteApprovedBy = User.Identity.Name;
                emrRequestDb.RequestStatus = 9;

                var vendorName = _wdb.Vendors.FirstOrDefault(t => t.VendorID == emrRequestDb.VendorID)?.Name;


                var note = new EmrNote() { EmrRequestID = emrRequestDb.EMRRequestID, PlacedBy = User.Identity.Name, DatePlaced = DateTime.Now, NoteContents = "Quote has been approved" };
                _wdb.EmrNotes.Add(note);
                await _wdb.SaveChangesAsync();

                var users = await _userManager.GetUsersInRoleAsync("Account Management");

                string FileData = await GeneratePDFAsync(emrRequestDb.EMRRequestID, "");
                foreach (var user in users)
                {
                    //if (user.Email.ToLower() != "max.khutoretsky@accureference.com")
                    //    await _emailSender.SendEmrRequestQuoteApproved(sb.ToString(), Statuses[emrRequestDb.RequestStatus].ToString(), user.Email, User.Identity.Name, "", "");
                }

                if (emrRequestDb != null)
                {
                    sb.Append("<htm><body><p>");
                    sb.Append($"EMR Request<br />");
                    sb.Append($"RequestId: {emrRequestDb.EMRRequestID} <br />");
                    sb.Append($"Account: {emrRequestDb.AccountID} <br />");
                    sb.Append($"Request Status: {Statuses[emrRequestDb.RequestStatus].ToString()} <br />");
                    sb.Append($"Office Manager Name: {emrRequestDb.OfficeManagerName} <br />");
                    sb.Append($"Office Phone: {emrRequestDb.OfficeManagerPhone} <br />");
                    sb.Append($"Physician Name: {emrRequestDb.PhysicianName} <br />");
                    sb.Append($"Physician Specialty: {emrRequestDb.PhysicianSpecialty} <br />");
                    sb.Append($"Expected Specimen Count: {emrRequestDb.ExpectedSpecimenCount} <br />");
                    if (emrRequestDb.HasCustomPanels)
                    {
                        sb.Append($"Custom Panels: {emrRequestDb.CustomPanels} <br />");
                    }
                    sb.Append($"Vendor Name: {(vendorName)} <br />");
                    sb.Append($"Vendor Contact: {emrRequestDb.VendorContact} <br />");
                    sb.Append($"Vendor Phone: {emrRequestDb.VendorPhone} <br />");
                    sb.Append($"Vendor Fax: {emrRequestDb.VendorFax} <br />");
                    sb.Append($"Vendor Email: {emrRequestDb.VendorEmail} <br />");
                    sb.Append($"Vendor ConnectionType: {emrRequestDb.ConnectionType} <br />");
                    sb.Append($"Requested By: {emrRequestDb.RequestedBy} <br />");
                    sb.Append($"Requested Date: {emrRequestDb.RequestedDate} <br />");
                    if (!string.IsNullOrEmpty(emrRequestDb.DenialReason))
                    {
                        sb.Append($"DenialReason: {emrRequestDb.DenialReason} <br />");
                    }

                    if (emrRequestDb.EmrNotes.Count > 0)
                    {
                        sb.Append($"<table class='table table-bordered table-hover table-striped table-sm'>");
                        sb.Append($"<tr><td>Action Taken</td><td>Action Taken By</td><td>Action Taken Date</td></tr>");
                        foreach (var action in emrRequestDb.EmrNotes.OrderByDescending(x => x.EmrNoteID).ToList())
                        {
                            sb.Append($"<tr><td width='45%'>");
                            sb.Append($" {action.NoteContents} </td>");
                            sb.Append($"<td width='35%'>");
                            sb.Append($" {action.PlacedBy} </td>");
                            sb.Append($"<td width='20%'>");
                            sb.Append($" {action.DatePlaced} </td></tr>");

                        }
                        sb.Append(" </table>");
                    }
                    sb.Append("</p></body></htm>");
                }
            }
            else
            {
                emrRequestDb.RequestStatus = 2;
                emrRequestDb.DenialReason = DenialReason;
                string FileData = await GeneratePDFAsync(emrRequestDb.EMRRequestID, DenialReason);
                var note = new EmrNote() { EmrRequestID = emrRequestDb.EMRRequestID, PlacedBy = User.Identity.Name, DatePlaced = DateTime.Now, NoteContents = DenialReason };
                _wdb.EmrNotes.Add(note);
                await _wdb.SaveChangesAsync();

                //await _emailSender.SendEmrRequestDeniedNotification(sb.ToString(), Statuses[emrRequestDb.RequestStatus].ToString(), emrRequestDb.RequestedBy, User.Identity.Name, "", "");
            }

            return Content(JsonConvert.SerializeObject("OK"));
        }

        public async Task<IActionResult> SaveEMRRequest1
                   (
                       int? EmrRequestID,
                       int Status,
                       int AccountID,
                       int VendorID,
                       string OfficeManagerName,
                       string OfficeManagerEmail,
                       string OfficeManagerPhone,
                       string PhysicianName,
                       string PhysicianSpecialty,
                       int ExpectedSpecimenCount,
                       bool HasCustomPanels,
                       string CustomPanels,
                       string VendorContact,
                       string VendorPhone,
                       string VendorFax,
                       string VendorEmail,
                       string ConnectionType,
                       string ActionTaken,
                       DateTime? ProjectedLiveDate,
                       DateTime? ActualLiveDate,
                       DateTime? ComtronTerminationDate,
                       DateTime? VendorTerminationDate,
                       bool? Approve,
                       string DenialReason
                   )
        {

            var emrRequestDb = _wdb.EmrRequests.FirstOrDefault(t => EmrRequestID.HasValue && t.EMRRequestID == EmrRequestID.Value);

            if (emrRequestDb == null)
                emrRequestDb = new EmrRequest();

            var prevStatus = emrRequestDb.RequestStatus;

            emrRequestDb.AccountID = AccountID;
            emrRequestDb.ConnectionType = ConnectionType;
            emrRequestDb.CustomPanels = CustomPanels;
            emrRequestDb.ExpectedSpecimenCount = ExpectedSpecimenCount;
            emrRequestDb.HasCustomPanels = HasCustomPanels;
            emrRequestDb.OfficeManagerEmail = OfficeManagerEmail;
            emrRequestDb.OfficeManagerName = OfficeManagerName;
            emrRequestDb.OfficeManagerPhone = OfficeManagerPhone;
            emrRequestDb.PhysicianName = PhysicianName;
            emrRequestDb.PhysicianSpecialty = PhysicianSpecialty;
            emrRequestDb.VendorContact = VendorContact;
            emrRequestDb.VendorEmail = VendorEmail;
            emrRequestDb.VendorFax = VendorFax;
            emrRequestDb.VendorID = VendorID;
            emrRequestDb.VendorPhone = VendorPhone;
            emrRequestDb.RequestStatus = Status;
            emrRequestDb.RequestedDate = DateTime.Now;
            emrRequestDb.ProjectedLiveDate = ProjectedLiveDate;
            emrRequestDb.ActualLiveDate = ActualLiveDate;
            emrRequestDb.ComtronTerminationDate = ComtronTerminationDate;
            emrRequestDb.VendorTerminationDate = VendorTerminationDate;

            _wdb.SaveChanges();


            //action taken 

            var emrNote = new EmrNote
            {
                DatePlaced = DateTime.Now,
                EmrRequestID = emrRequestDb.EMRRequestID,
                NoteContents = ActionTaken,
                PlacedBy=User.Identity.Name
            };

            _wdb.SaveChanges();

            StringBuilder sb = new StringBuilder();


            var vendorName = _wdb.Vendors.FirstOrDefault(t => t.VendorID == emrRequestDb.VendorID)?.Name;

            emrRequestDb = _wdb.EmrRequests.Include(t => t.EmrNotes).FirstOrDefault(t => t.EMRRequestID == EmrRequestID);

            if (emrRequestDb != null)
            {
                sb.Append("<htm><body><p>");
                sb.Append($"EMR Request<br />");
                sb.Append($"RequestId: {emrRequestDb.EMRRequestID} <br />");
                sb.Append($"Account: {emrRequestDb.AccountID} <br />");
                sb.Append($"Request Status: {Statuses[emrRequestDb.RequestStatus].ToString()} <br />");
                sb.Append($"Office Manager Name: {emrRequestDb.OfficeManagerName} <br />");
                sb.Append($"Office Phone: {emrRequestDb.OfficeManagerPhone} <br />");
                sb.Append($"Physician Name: {emrRequestDb.PhysicianName} <br />");
                sb.Append($"Physician Specialty: {emrRequestDb.PhysicianSpecialty} <br />");
                sb.Append($"Expected Specimen Count: {emrRequestDb.ExpectedSpecimenCount} <br />");
                if (emrRequestDb.HasCustomPanels)
                {
                    sb.Append($"Custom Panels: {emrRequestDb.CustomPanels} <br />");
                }
                sb.Append($"Vendor Name: {(vendorName)} <br />");
                sb.Append($"Vendor Contact: {emrRequestDb.VendorContact} <br />");
                sb.Append($"Vendor Phone: {emrRequestDb.VendorPhone} <br />");
                sb.Append($"Vendor Fax: {emrRequestDb.VendorFax} <br />");
                sb.Append($"Vendor Email: {emrRequestDb.VendorEmail} <br />");
                sb.Append($"Vendor ConnectionType: {emrRequestDb.ConnectionType} <br />");
                sb.Append($"Requested By: {emrRequestDb.RequestedBy} <br />");
                sb.Append($"Requested Date: {emrRequestDb.RequestedDate} <br />");
                if (!string.IsNullOrEmpty(emrRequestDb.DenialReason))
                {
                    sb.Append($"DenialReason: {emrRequestDb.DenialReason} <br />");
                }

                if (emrRequestDb.EmrNotes.Count > 0)
                {
                    sb.Append($"<table class='table table-bordered table-hover table-striped table-sm'>");
                    sb.Append($"<tr><td>Action Taken</td><td>Action Taken By</td><td>Action Taken Date</td></tr>");
                    foreach (var action in emrRequestDb.EmrNotes.OrderByDescending(x => x.EmrNoteID).ToList())
                    {
                        sb.Append($"<tr><td width='45%'>");
                        sb.Append($" {action.NoteContents} </td>");
                        sb.Append($"<td width='35%'>");
                        sb.Append($" {action.PlacedBy} </td>");
                        sb.Append($"<td width='20%'>");
                        sb.Append($" {action.DatePlaced} </td></tr>");

                    }
                    sb.Append(" </table>");
                }
                sb.Append("</p></body></htm>");
            }

            if (Status == prevStatus)
            {
                ActionTaken = ": An action has been taken";
            }
            else
            {
                ActionTaken = ":Status Changed To " + Statuses[emrRequestDb.RequestStatus].ToString();
                if (Statuses[emrRequestDb.RequestStatus].ToString().ToUpper() == "ACKNOWLEDGED")
                {
                    ActionTaken = "Acknowledged";
                }
                if (Statuses[emrRequestDb.RequestStatus].ToString().ToUpper() == "PENDING QUOTE APPROVAL" && emrRequestDb.AttachmentID > 0)
                {
                    ActionTaken = "Pending Quote Approval";
                }
            }

            if (emrRequestDb.RequestStatus > 1 && emrRequestDb.RequestStatus < 6)
            {
                //sending PDF whenever data is modified
                string FileData = await GeneratePDFAsync(emrRequestDb.EMRRequestID);
                //await _emailSender.SendEmrRequestCompletedNotification(EmrRequest.RequestedBy, User.Identity.Name);
                //await _emailSender.SendEmrRequestCompletedNotification(sb.ToString(), (!string.IsNullOrEmpty(ActionTaken) ? ActionTaken : ""), EmrRequest.RequestedBy, User.Identity.Name, "", "");
            }
            else
            {
                //sending PDF whenever data is modified
                string FileData = await GeneratePDFAsync(emrRequestDb.EMRRequestID);
                //await _emailSender.SendEmrRequestModifiedNotification(EmrRequest.RequestedBy, User.Identity.Name);
                //await _emailSender.SendEmrRequestModifiedNotification(sb.ToString(), (!string.IsNullOrEmpty(ActionTaken) ? ActionTaken : ""), EmrRequest.RequestedBy, User.Identity.Name, "", "");
            }

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public async Task<string> GeneratePDFAsync(int? EMRReqID, string DenialReason)
        {
            //var EmrRequest = await _wdb.EmrRequests
            //    .Include(e => e.Account).Include(e => e.Quote).Include(e => e.EmrNotes).SingleOrDefaultAsync(m => m.EmrRequestID == EMRReqID);

            var emrRequest = _wdb.EmrRequests.FirstOrDefault(t => t.EMRRequestID == EMRReqID);
            var attachment = _wdb.Attachments.FirstOrDefault(t => t.AttachmentID == emrRequest.AccountID);
            var notes = _wdb.EmrNotes.Where(t => t.EmrRequestID == emrRequest.EMRRequestID).ToList();
            var vendor = _wdb.Vendors.FirstOrDefault(t => t.VendorID == emrRequest.VendorID);

                        

            var fmodel = new EMRRequestViewModel();
            fmodel.emrRequest = emrRequest;
            fmodel.DenialReason = DenialReason;
            fmodel.emrNote = emrRequest.EmrNotes;
            fmodel.DenialReason = emrRequest.DenialReason;
            fmodel.VendorID = (vendor != null) ? vendor.VendorID : 0;
            fmodel.VendorName = (vendor != null) ? vendor.Name :string.Empty;
            //Getting group details to get name to print in PDF

            var testContent = await _templateService.RenderTemplateAsync("~/Views/EMRRequestPDFView.cshtml", fmodel);
            var doc =
            new HtmlToPdfDocument()
            {
                Objects =
                    {
                            new ObjectSettings()
                            {
                                PagesCount = true,
                                HtmlContent = testContent,
                                WebSettings = {DefaultEncoding = "utf-8" },
                                FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                            }
                    }
            };

            var output = _pdfConverter.Convert(doc);
            var file = Convert.ToBase64String(output);
            return file;
        }
        public async Task<string> GeneratePDFAsync(int? EMRReqID)
        {
            var EmrRequest = _wdb.EmrRequests.FirstOrDefault(t => t.EMRRequestID == EMRReqID);
            var attachment = _wdb.Attachments.FirstOrDefault(t => t.AttachmentID == EmrRequest.AccountID);
            var notes = _wdb.EmrNotes.Where(t => t.EmrRequestID == EmrRequest.EMRRequestID).ToList();
            var vendor = _wdb.Vendors.FirstOrDefault(t => t.VendorID == EmrRequest.VendorID);

            var fmodel = new EMRRequestViewModel();
            fmodel.emrRequest = EmrRequest;
            fmodel.emrNote = EmrRequest.EmrNotes;
            fmodel.DenialReason = EmrRequest.DenialReason;
            //Getting group details to get name to print in PDF

            var testContent = await _templateService.RenderTemplateAsync("~/Views/EMRRequestPDFView.cshtml", fmodel);
            var doc =
            new HtmlToPdfDocument()
            {
                Objects =
                    {
                            new ObjectSettings()
                            {
                                PagesCount = true,
                                HtmlContent = testContent,
                                WebSettings = {DefaultEncoding = "utf-8" },
                                FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                            }
                    }
            };

            var output = _pdfConverter.Convert(doc);
            var file = Convert.ToBase64String(output);
            return file;
        }
        public IActionResult AddNewVendor(string newVendorName)
        {
            var vendor = _wdb.Vendors.FirstOrDefault(t => t.Name.Trim().ToUpper() == newVendorName.Trim().ToUpper());
            if (vendor != null)
            {
                var data = new
                {
                    message = "Vendor is existing!",
                };

                return Content(JsonConvert.SerializeObject(data));

            } else
            {
                vendor = new Vendor
                {
                     Name= newVendorName.Trim(),
                     ComtronInstallationCost =null,
                     VendorAnnualCost=null,
                     VendorInstallationCost=null,
                     VendorMaintenanceCost=null
                };
                _wdb.Vendors.Add(vendor);
                _wdb.SaveChanges();

                var data = new
                {
                    message = "OK",
                    Name= newVendorName.Trim(),
                    ID=vendor.VendorID
                };

                return Content(JsonConvert.SerializeObject(data));

            }
        }
        public IActionResult DeleteEMRRequest(int emrRequestId)
        {
            var emrRequest = _wdb.EmrRequests.FirstOrDefault(t => t.EMRRequestID == emrRequestId);
            if(emrRequest!=null)
            {
                var emrNotes = _wdb.EmrNotes.Where(t => t.EmrRequestID == emrRequestId).ToList();
                var emrAttach = _wdb.Attachments.FirstOrDefault(t => t.AttachmentID == emrRequest.AttachmentID);

                _wdb.EmrNotes.RemoveRange(emrNotes);
                _wdb.Attachments.Remove(emrAttach);

                _wdb.EmrRequests.Remove(emrRequest);
                _wdb.SaveChanges();
            }
            return Content(JsonConvert.SerializeObject("OK"));
        }
        public IActionResult SaveQuoteRequest(int emrRequestId, string filename, string content)
        {


            StringBuilder sb = new StringBuilder();
            string ActionTaken = string.Empty;

            var request = _wdb.EmrRequests.FirstOrDefault(m => m.EMRRequestID == emrRequestId);


            if (request.AttachmentID.HasValue)
            {
                var attach = _wdb.Attachments.FirstOrDefault(m => m.AttachmentID == request.AttachmentID.Value);
                attach.AttachmentContent = content;
                attach.AttachmentName = filename;
                _wdb.SaveChanges();
            }
            else
            {
                var attach = new Attachment() { AttachmentContent = content, AttachmentName = filename };
                _wdb.Attachments.Add(attach);
                _wdb.SaveChanges();
                request.AttachmentID = attach.AttachmentID;
            }

            request.QuoteApprovedDate = null;
            request.QuoteApprovedBy = null;

            request.RequestStatus = 8;

            var note = new EmrNote()
            {
                EmrRequestID = request.EMRRequestID,
                PlacedBy = User.Identity.Name,
                DatePlaced = DateTime.Now,
                NoteContents = "Quote has been attached and is pending approval"
            };

            _wdb.EmrNotes.Add(note);
            _wdb.SaveChanges();

            var users = _userManager.GetUsersInRoleAsync("Finance").Result;

            foreach (var user in users)
            {
                //if (user.Email.ToLower() != "max.khutoretsky@accureference.com")
                //    await _emailSender.SendEmrRequestQuoteUploaded(user.Email, User.Identity.Name);
            }

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public IActionResult SaveApproveRequest(int emrRequestId,bool? Approved,string DenialReason)
        {

            StringBuilder sb = new StringBuilder();
            string ActionTaken = string.Empty;
            var request = _wdb.EmrRequests.FirstOrDefault(m => m.EMRRequestID == emrRequestId);

            //var EmrRequest = _wdb.EmrRequests.Include(e => e.Account).Include(e => e.Quote).Include(e => e.EmrNotes).SingleOrDefaultAsync(m => m.EmrRequestID == emrRequestId);

            var EmrRequest = _wdb.EmrRequests.FirstOrDefault(m => m.EMRRequestID == emrRequestId);
            var Account = _wdb.Accounts.FirstOrDefault(t => t.AccountID == EmrRequest.AccountID);
            var EmrNotes = _wdb.EmrNotes.Where(t => t.EmrRequestID == EmrRequest.EMRRequestID).ToList();
            var Vendor = _wdb.Vendors.FirstOrDefault(t => t.VendorID == EmrRequest.VendorID);


            //create body string for email here
            if (EmrRequest != null)
            {

                sb.Append("<htm><body><p>");
                sb.Append($"EMR Request<br />");
                sb.Append($"RequestId: {EmrRequest.EMRRequestID} <br />");
                sb.Append($"Account: {EmrRequest.Account.AccountID} <br />");
                sb.Append($"Request Status: {Statuses[EmrRequest.RequestStatus].ToString()} <br />");
                sb.Append($"Office Manager Name: {EmrRequest.OfficeManagerName} <br />");
                sb.Append($"Office Phone: {EmrRequest.OfficeManagerPhone} <br />");
                sb.Append($"Physician Name: {EmrRequest.PhysicianName} <br />");
                sb.Append($"Physician Specialty: {EmrRequest.PhysicianSpecialty} <br />");
                sb.Append($"Expected Specimen Count: {EmrRequest.ExpectedSpecimenCount} <br />");
                if (EmrRequest.HasCustomPanels)
                {
                    sb.Append($"Custom Panels: {EmrRequest.CustomPanels} <br />");
                }
                sb.Append($"Vendor Name: {(Vendor != null ? Vendor.Name : "")} <br />");
                sb.Append($"Vendor Contact: {EmrRequest.VendorContact} <br />");
                sb.Append($"Vendor Phone: {EmrRequest.VendorPhone} <br />");
                sb.Append($"Vendor Fax: {EmrRequest.VendorFax} <br />");
                sb.Append($"Vendor Email: {EmrRequest.VendorEmail} <br />");
                sb.Append($"Vendor ConnectionType: {EmrRequest.ConnectionType} <br />");
                sb.Append($"Requested By: {EmrRequest.RequestedBy} <br />");
                sb.Append($"Requested Date: {EmrRequest.RequestedDate} <br />");
                if (!string.IsNullOrEmpty(EmrRequest.DenialReason))
                {
                    sb.Append($"DenialReason: {EmrRequest.DenialReason} <br />");
                }

                if (EmrNotes.Count > 0)
                {
                    sb.Append($"<table class='table table-bordered table-hover table-striped table-sm'>");
                    sb.Append($"<tr><td>Action Taken</td><td>Action Taken By</td><td>Action Taken Date</td></tr>");
                    foreach (var action in EmrRequest.EmrNotes.OrderByDescending(x => x.EmrNoteID).ToList())
                    {
                        sb.Append($"<tr><td width='45%'>");
                        sb.Append($" {action.NoteContents} </td>");
                        sb.Append($"<td width='35%'>");
                        sb.Append($" {action.PlacedBy} </td>");
                        sb.Append($"<td width='20%'>");
                        sb.Append($" {action.DatePlaced} </td></tr>");

                    }
                    sb.Append(" </table>");
                }

                sb.Append("</p></body></htm>");

            }

            if (Approved.HasValue && Approved.Value)
            {
                request.QuoteApprovedDate = DateTime.Now;
                request.QuoteApprovedBy = User.Identity.Name;
                request.RequestStatus = 9;
                var note = new EmrNote() { EmrRequestID = request.EMRRequestID, PlacedBy = User.Identity.Name, DatePlaced = DateTime.Now, NoteContents = "Quote has been approved" };
                _wdb.EmrNotes.Add(note);
                _wdb.SaveChanges();

                var users = _userManager.GetUsersInRoleAsync("Account Management").Result;
                //var users = NotificationExtension.GetUsersSubscribedOnNotification("SendEmrRequestQuoteApproved", _context);
                string FileData = GeneratePDFAsync(EmrRequest.EMRRequestID, "").Result;
                foreach (var user in users)
                {
                    //if(user.Email.ToLower()!= "max.khutoretsky@accureference.com")
                    //    await _emailSender.SendEmrRequestQuoteApproved(sb.ToString(), Statuses[EmrRequest.RequestStatus].ToString(), user.Email, User.Identity.Name, "", "");
                }
            }
            else
            {
                request.RequestStatus = 2;
                request.DenialReason = DenialReason;
                string FileData = GeneratePDFAsync(request.EMRRequestID, DenialReason).Result;
                var note = new EmrNote() { EmrRequestID = request.EMRRequestID, PlacedBy = User.Identity.Name, DatePlaced = DateTime.Now, NoteContents = DenialReason };
                _wdb.EmrNotes.Add(note);
                _wdb.SaveChanges();

                //if (bEmrRequestDeniedNotification)
                //{
                _emailSender.SendEmrRequestDeniedNotification(sb.ToString(), Statuses[EmrRequest.RequestStatus].ToString(), request.RequestedBy, User.Identity.Name, "", "");
                //await _emailSender.SendEmrRequestDeniedNotification(request.RequestedBy, User.Identity.Name);
                //}
            }

            return Content(JsonConvert.SerializeObject("OK"));
        }
    }
}
