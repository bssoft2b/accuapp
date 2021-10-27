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

    public class AccountRequestController : Controller
    {
        private readonly WebdbContext _wdb;
        private readonly WebdbContextProcedures _contextProcedureWebDb;
        private readonly PhlebotomyContext _ph;
        private readonly UserManager<ApplicationUser> _userInManager;

        private readonly IEmailSender _emailSender;
        private readonly ITemplateService _templateService;
        private readonly IConverter _pdfConverter;


        public List<String> Options { get; set; } = new List<string>() { "", "Auto Print", "EMR", "Fax", "Hard Copy", "Portal" };
        public List<String> ReqTypeList { get; set; } = new List<string>() { "", "New Practice/Physician", "Add Physician to Existing Account", "New PSC", "New Nursing Home" };
        public List<String> RequestStatusList { get; set; } = new List<string>() { "New", "Completed", "Denied", "Under Different Group", "Account Closed" };

        public AccountRequestController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext contextPhlebotomy, IEmailSender emailSender,
            ITemplateService templateService, IConverter pdfConverter)
        {
            _wdb = contextWebDb;
            _contextProcedureWebDb = contextProcedureWebDb;
            _ph = contextPhlebotomy;
            _templateService = templateService;
            _pdfConverter = pdfConverter;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View("AccountRequest");
        }
        public IActionResult AccountRequestListJson(int requestStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var Claims = User.Claims.Where(t => t.Type == "SalesRep").Select(t => t.Value).ToList();
            var Groups = User.Claims.Where(t => t.Type == "Group").Select(t => t.Value).ToList();

            IQueryable<AccountRequest> accountRequests;

            accountRequests = _wdb.AccountRequests.Include(x=>x.Group).Where(t => (Claims.Contains(t.Group.SalesRep) || Claims.Contains(t.Group.Manager) || Groups.Contains(t.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager")) && (t.RequestStatus == requestStatus || requestStatus==-1))
                .OrderBy(t => t.RequestDate).Select(t => t);

            var recordsTotal = accountRequests.Count();
            var recordsFiltered = 0;

            if (!string.IsNullOrEmpty(searchValue))
            {
                accountRequests = accountRequests.Where(t => Convert.ToString(t.AccountRequestID).Contains(searchValue) || t.PracticeName.Contains(searchValue));
                recordsFiltered = accountRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.RequestDate) : accountRequests.OrderByDescending(t => t.RequestDate);
            }
            if (orderColumn == 1)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.PracticeName) : accountRequests.OrderByDescending(t => t.PracticeName);
            }
            if (orderColumn == 2)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Address) : accountRequests.OrderByDescending(t => t.Address);
            }
            if (orderColumn == 3)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Suite) : accountRequests.OrderByDescending(t => t.Suite);
            }
            if (orderColumn == 4)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.City) : accountRequests.OrderByDescending(t => t.City);
            }
            if (orderColumn == 5)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.State) : accountRequests.OrderByDescending(t => t.State);
            }
            if (orderColumn == 6)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Zip) : accountRequests.OrderByDescending(t => t.Zip);
            }
            if (orderColumn == 7)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Phone) : accountRequests.OrderByDescending(t => t.Phone);
            }
            if (orderColumn == 8)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.OfficeContactName) : accountRequests.OrderByDescending(t => t.OfficeContactName);
            }
            if (orderColumn == 9)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.ContactEmail) : accountRequests.OrderByDescending(t => t.ContactEmail);
            }
            if (orderColumn == 10)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.GroupID) : accountRequests.OrderByDescending(t => t.GroupID);
            }
            if (orderColumn == 11)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Group.SalesRep) : accountRequests.OrderByDescending(t => t.Group.SalesRep);
            }
            if (orderColumn == 12)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.RequestedBy) : accountRequests.OrderByDescending(t => t.RequestedBy);
            }

            var accountRequestsList = accountRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = accountRequestsList.Select(t=>new { 
                    t.AccountRequestID,
                    t.RequestDate,
                    t.PracticeName,
                    t.Address,
                    t.Suite,
                    t.City,
                    t.State,
                    t.Zip,
                    t.Phone,
                    t.OfficeContactName,
                    t.ContactEmail,
                    t.GroupID,
                    t.Group.SalesRep,
                    t.RequestedBy,
                    RequestStatusName=t.RequestStatus==0?"New":(t.RequestStatus == 1? "Completed" : (t.RequestStatus == 2? "Under Different Group" : (t.RequestStatus == 3? "Account Closed" :""))),
                    t.RequestStatus,
                    t.ClientIDS,
                    t.CompletedBy,
                    t.CompletedDate
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult NewAccountRequests()
        {

            var Claims = User.Claims.Where(c => c.Type == "SalesRep").Select(c => c.Value).ToList();
            var Groups = User.Claims.Where(c => c.Type == "Group").Select(c => c.Value).ToList();
            var Group = _wdb.Groups.Where(g => Claims.Contains(g.SalesRep) || Claims.Contains(g.Manager) || Groups.Contains(g.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager")).ToList();

            Dictionary<int, string> gd = new Dictionary<int, string>();

            foreach(var g in Group)
            {
                gd.Add(g.GroupID, g.GroupID.ToString() + " - " + g.Name);
            }

            ViewBag.Group = gd;

            return View("NewAccountRequests");
        }
        public IActionResult NewAccountRequestsJson()
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var Claims = User.Claims.Where(t => t.Type == "SalesRep").Select(t => t.Value).ToList();
            var Groups = User.Claims.Where(t => t.Type == "Group").Select(t => t.Value).ToList();

            IQueryable<AccountRequest> accountRequests;

            accountRequests = _wdb.AccountRequests.Include(x => x.Group).Where(t => (Claims.Contains(t.Group.SalesRep) || Claims.Contains(t.Group.Manager) || Groups.Contains(t.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager")) && (t.RequestStatus ==0))
                .OrderBy(t => t.RequestDate).Select(t => t);

            var recordsTotal = accountRequests.Count();
            var recordsFiltered = 0;

            if (!string.IsNullOrEmpty(searchValue))
            {
                accountRequests = accountRequests.Where(t => Convert.ToString(t.AccountRequestID).Contains(searchValue) || t.PracticeName.Contains(searchValue));
                recordsFiltered = accountRequests.Count();
            }
            else
            {
                recordsFiltered = recordsTotal;
            }

            if (orderColumn == 0)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.RequestDate) : accountRequests.OrderByDescending(t => t.RequestDate);
            }
            if (orderColumn == 1)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.PracticeName) : accountRequests.OrderByDescending(t => t.PracticeName);
            }
            if (orderColumn == 2)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Address) : accountRequests.OrderByDescending(t => t.Address);
            }
            if (orderColumn == 3)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Suite) : accountRequests.OrderByDescending(t => t.Suite);
            }
            if (orderColumn == 4)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.City) : accountRequests.OrderByDescending(t => t.City);
            }
            if (orderColumn == 5)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.State) : accountRequests.OrderByDescending(t => t.State);
            }
            if (orderColumn == 6)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Zip) : accountRequests.OrderByDescending(t => t.Zip);
            }
            if (orderColumn == 7)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Phone) : accountRequests.OrderByDescending(t => t.Phone);
            }
            if (orderColumn == 8)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.OfficeContactName) : accountRequests.OrderByDescending(t => t.OfficeContactName);
            }
            if (orderColumn == 9)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.ContactEmail) : accountRequests.OrderByDescending(t => t.ContactEmail);
            }
            if (orderColumn == 10)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.GroupID) : accountRequests.OrderByDescending(t => t.GroupID);
            }
            if (orderColumn == 11)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.Group.SalesRep) : accountRequests.OrderByDescending(t => t.Group.SalesRep);
            }
            if (orderColumn == 12)
            {
                accountRequests = (orderDirect == "asc") ? accountRequests.OrderBy(t => t.RequestedBy) : accountRequests.OrderByDescending(t => t.RequestedBy);
            }

            var accountRequestsList = accountRequests.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = accountRequestsList.Select(t => new {
                    t.AccountRequestID,
                    t.RequestDate,
                    t.ClientType,
                    t.PracticeName,
                    t.Address,
                    t.Suite,
                    t.City,
                    t.State,
                    t.Zip,
                    t.Phone,
                    t.OfficeContactName,
                    t.ContactEmail,
                    t.GroupID,
                    t.Group.SalesRep,
                    t.RequestedBy
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetAccountRequest(int? accountRequestId)
        {
            var accountRequest = _wdb.AccountRequests.Include(t=>t.PhysicianRequests).FirstOrDefault(t=>t.AccountRequestID==accountRequestId);

            if (accountRequest != null)
            {
                var ResultDeliveryOptionList =string.Join(",",_wdb.ResultDeliveryOptions.Where(t => t.AccountRequestID == accountRequestId)
                    .Select(t =>Convert.ToString(t.OptionID)));

                var Claims = User.Claims.Where(c=>c.Type == "SalesRep").Select(c=>c.Value).ToList();
                var Groups=User.Claims.Where(c=>c.Type == "Group").Select(c=>c.Value).ToList();
                var Physician = _wdb.PhysicianRequests.Where(t => t.AccountRequestID == accountRequestId).Select(t=>new { 
                    t.PhysicianRequestID,
                    t.PhysicianName,
                    t.PhysicianNPI
                }).ToArray();
                var Group = _wdb.Groups.Where(g => Claims.Contains(g.SalesRep) || Claims.Contains(g.Manager) || Groups.Contains(g.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager"))
                    .Select(t=>new
                    {
                        t.GroupID,
                        t.Manager,
                        t.Name,
                        t.Negative,
                        t.SalesRep,
                        t.Telephone,
                        t.Active
                    }).ToList();

                var data = new
                {
                    accountRequest.AccountRequestID,
                    accountRequest.Address,
                    accountRequest.Auto,
                    accountRequest.City,
                    accountRequest.ClientIDS,
                    accountRequest.ClientType,
                    accountRequest.CompletedBy,
                    accountRequest.CompletedDate,
                    accountRequest.ContactEmail,
                    accountRequest.ContactEmail1,
                    accountRequest.EMRVendor,
                    accountRequest.ExistingAccountID,
                    accountRequest.Fax,
                    accountRequest.GroupID,
                    accountRequest.NeedPgxPortal,
                    accountRequest.OfficeContactName,
                    accountRequest.Phone,
                    accountRequest.PhysicianEmail,
                    accountRequest.PracticeName,
                    accountRequest.PracticeNPI,
                    accountRequest.RequestDate,
                    accountRequest.RequestedBy,
                    accountRequest.RequestStatus,
                    accountRequest.RequestType,
                    accountRequest.ResultDeliveryOption,
                    ResultDeliveryOptionList,
                    accountRequest.State,
                    accountRequest.Suite,
                    accountRequest.SuppliesNeeded,
                    accountRequest.Zip,
                    Physician
                };

                return Content(JsonConvert.SerializeObject(data));
            }
            return Content(JsonConvert.SerializeObject("Error"));
        }
        public async Task<IActionResult> SaveAccountRequest(
                int AccountRequestID,
                int GroupID,
                int RequestType,
                int ExistingAccountID,
                int ClientType,
                string PracticeName,
                string PracticeNPI,
                int[] ResultDeliveryOption,
                string EMRVendor,
                bool? Auto,
                string Address,
                string Suite,
                string City,
                string State,
                string Zip,
                string Phone,
                string Fax,
                string OfficeContactName,
                string ContactEmail,
                string ContactEmail1,
                bool SuppliesNeeded,
                bool NeedPgxPortal,
                string PhysicianEmail,
                int?[] PhysIDs,
                string[] PhysNames,
                string[] PhysNPIs,
                int Status
            )
        {


            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var group = from g in _wdb.Groups
                        where Claims.Contains(g.SalesRep) || Claims.Contains(g.Manager) || Groups.Contains(g.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager")
                        select g;
            var Group = group.ToList();

            StringBuilder sb = new StringBuilder();
            string DeliveryOption = string.Empty;

            AccountRequest accountRequestOld = _wdb.AccountRequests.AsNoTracking().FirstOrDefault(a => a.AccountRequestID == AccountRequestID);
            List<ResultDeliveryOption> resultDeliveryOptionOld = null;
            List<PhysicianRequest> physiciansOld = null;

            AccountRequest accountRequestUpdate = new AccountRequest();
            List<ResultDeliveryOption> resultDeliveryOptionUpdate = new List<ResultDeliveryOption>();
            List<PhysicianRequest> physiciansUpdate = new List<PhysicianRequest>();



            if (accountRequestOld != null)
            {
                resultDeliveryOptionOld = _wdb.ResultDeliveryOptions.Where(t => t.AccountRequestID == AccountRequestID).ToList();
                physiciansOld = _wdb.PhysicianRequests.Where(t => t.AccountRequestID == AccountRequestID).ToList();

                accountRequestUpdate.Address = Address;
                accountRequestUpdate.Auto = Auto;
                accountRequestUpdate.City = City;
                accountRequestUpdate.ClientType = ClientType;
                accountRequestUpdate.ClientIDS = accountRequestOld.ClientIDS;
                accountRequestUpdate.CompletedBy = (accountRequestOld.RequestStatus == 0 && Status == 1) ? User.Identity.Name: accountRequestOld.CompletedBy;
                accountRequestUpdate.CompletedDate = (accountRequestOld.RequestStatus == 0 && Status == 1) ? DateTime.Now:accountRequestOld.CompletedDate;
                accountRequestUpdate.ContactEmail = ContactEmail;
                accountRequestUpdate.ContactEmail1 = ContactEmail1;
                accountRequestUpdate.EMRVendor = EMRVendor;
                accountRequestUpdate.ExistingAccountID = ExistingAccountID;
                accountRequestUpdate.Fax = Fax;
                accountRequestUpdate.GroupID = GroupID;
                accountRequestUpdate.NeedPgxPortal = NeedPgxPortal;
                accountRequestUpdate.OfficeContactName = OfficeContactName;
                accountRequestUpdate.Phone = Phone;
                accountRequestUpdate.PhysicianEmail = PhysicianEmail;
                accountRequestUpdate.PracticeName = PracticeName;
                accountRequestUpdate.PracticeNPI = PracticeNPI;
                accountRequestUpdate.RequestDate = accountRequestOld.RequestDate;
                accountRequestUpdate.RequestedBy = accountRequestOld.RequestedBy;
                accountRequestUpdate.RequestStatus = Status;
                accountRequestUpdate.RequestType = RequestType;
                accountRequestUpdate.State = State;
                accountRequestUpdate.Suite = Suite;
                accountRequestUpdate.SuppliesNeeded = SuppliesNeeded;
                accountRequestUpdate.WebUserName = accountRequestOld.WebUserName;
                accountRequestUpdate.WebUserPassword = accountRequestOld.WebUserPassword;
                accountRequestUpdate.Zip = Zip;

            }
            else
            {
                accountRequestUpdate = new AccountRequest
                {
                    Address = Address,
                    Auto = Auto,
                    City = City,
                    ClientType = ClientType,
                    ContactEmail = ContactEmail,
                    ContactEmail1 = ContactEmail1,
                    EMRVendor = EMRVendor,
                    ExistingAccountID = ExistingAccountID,
                    Fax = Fax,
                    GroupID = GroupID,
                    NeedPgxPortal = NeedPgxPortal,
                    OfficeContactName = OfficeContactName,
                    Phone = Phone,
                    PhysicianEmail = PhysicianEmail,
                    PracticeName = PracticeName,
                    PracticeNPI = PracticeNPI,
                    RequestedBy=User.Identity.Name,
                    RequestDate=DateTime.Now,
                    RequestStatus = Status,
                    RequestType = RequestType,
                    State = State,
                    Suite = Suite,
                    SuppliesNeeded = SuppliesNeeded,
                    //WebUserName = 
                    //WebUserPassword = 
                    Zip = Zip
                };
            }

            try
            {
                _wdb.AccountRequests.Add(accountRequestUpdate);
                _wdb.SaveChanges();

                if(physiciansOld!=null)
                    _wdb.PhysicianRequests.RemoveRange(physiciansOld);
                if(resultDeliveryOptionOld!=null)
                    _wdb.ResultDeliveryOptions.RemoveRange(resultDeliveryOptionOld);
                if (accountRequestOld != null)
                    _wdb.AccountRequests.Remove(accountRequestOld);

                _wdb.SaveChanges();

            }catch(Exception ex)
            {

            }

            var existQuery = from a in _wdb.AccountRequests where (a.Address.ToUpper() == Address.ToUpper() && a.Suite.ToUpper() == Suite.ToUpper() && a.City.ToUpper() == City.ToUpper()) select a;
            var existChecker = existQuery.ToList();
            var existQuery2 = from a in _wdb.Accounts where (a.Address.ToUpper() == Address.ToUpper() && a.Suite.ToUpper() == Suite.ToUpper() && a.City.ToUpper() == City.ToUpper() && a.Active == true) select a;
            var existChecker2 = existQuery2.ToList();

            foreach (var option in ResultDeliveryOption)
            {
                _wdb.ResultDeliveryOptions.Add(new ResultDeliveryOption() { AccountRequestID = accountRequestUpdate.AccountRequestID, OptionID = option });
            }

            for(var i=0;i<PhysIDs.Length;i++)
            {
                _wdb.PhysicianRequests.Add(new PhysicianRequest() { AccountRequestID = accountRequestUpdate.AccountRequestID, PhysicianName=PhysNames[i], PhysicianNPI=PhysNPIs[i] });
            }

            _wdb.SaveChanges();

            var Result = _wdb.ResultDeliveryOptions.Where(t => t.AccountRequestID == accountRequestUpdate.AccountRequestID).ToList();

            string FileData = await GeneratePDFAsync(accountRequestUpdate.AccountRequestID);

            if (accountRequestUpdate != null)
            {


                sb.Append("<htm><body><p>");
                sb.Append($"Account Request<br /> ");
                sb.Append($"RequestId: {accountRequestUpdate.AccountRequestID} <br /> ");
                sb.Append($"Status: Modified <br /> ");
                //sb.Append($"Request Status: {Statuses[EmrRequest.RequestStatus].ToString()} <br /><br />");
                sb.Append($"Group Name: {Group.Where(x => x.GroupID == accountRequestUpdate.GroupID).FirstOrDefault().Name} <br />");
                sb.Append($"ClientIDs: {(string.IsNullOrEmpty(accountRequestUpdate.ClientIDS) ? "" : accountRequestUpdate.ClientIDS)} <br />");
                sb.Append($"RequestType: {ReqTypeList[accountRequestUpdate.RequestType].ToString()} <br />");
                if (ReqTypeList[accountRequestUpdate.RequestType].ToString() == "Add Physician to Existing Account")
                {
                    sb.Append($"Existing AccountID : {accountRequestUpdate.ExistingAccountID} <br /> ");
                }

                sb.Append($"PracticeName : {accountRequestUpdate.PracticeName} <br />");
                sb.Append($"PracticeNPI : {accountRequestUpdate.PracticeNPI} <br />");


                if (accountRequestUpdate.PhysicianRequests.Count > 0)
                {
                    sb.Append($"<table class='table table-bordered table-hover table-striped table-sm'>");
                    sb.Append($"<tr><td>PhysicianName</td><td>PhysicianNPI</td></tr>");
                    foreach (var action in accountRequestUpdate.PhysicianRequests)
                    {
                        sb.Append($"<tr><td width='45%'>");
                        sb.Append($" {action.PhysicianName} </td>");
                        sb.Append($"<td width='35%'>");
                        sb.Append($" {action.PhysicianNPI} </td></tr>");
                    }
                    sb.Append(" </table>");
                }

                if (Result.Count > 0)
                {
                    foreach (var item in Result)
                    {
                        DeliveryOption = DeliveryOption + ", " + Options[item.OptionID].ToString();
                    }
                }
                sb.Append($"Delivery Option : {DeliveryOption.Trim(',')} <br />");

                if (DeliveryOption.Contains("Fax"))
                {
                    sb.Append($"Auto: {(accountRequestUpdate.Auto == true ? "Automatic" : "Manual")} <br />");
                }
                sb.Append($"Address: {accountRequestUpdate.Address} <br />");
                sb.Append($"Suite: {accountRequestUpdate.Suite} <br />");
                sb.Append($"City: {accountRequestUpdate.City} <br />");
                sb.Append($"State: {accountRequestUpdate.State} <br />");
                sb.Append($"Zip: {accountRequestUpdate.Zip} <br />");
                sb.Append($"Phone: {accountRequestUpdate.Phone} <br />");
                sb.Append($"Fax: {accountRequestUpdate.Fax} <br />");
                sb.Append($"Office ContactName: {accountRequestUpdate.OfficeContactName} <br />");
                sb.Append($"Contact Email: {accountRequestUpdate.ContactEmail} <br />");
                sb.Append($"SuppliesNeeded: {accountRequestUpdate.SuppliesNeeded} <br />");
                sb.Append($"NeedPgxPortal: {(accountRequestUpdate.NeedPgxPortal == true ? "Yes" : "No")} <br />");

                if (accountRequestUpdate.NeedPgxPortal)
                {
                    sb.Append($"Physician Email: {accountRequestUpdate.PhysicianEmail} <br />");
                }
                sb.Append($"RequestedBy: {accountRequestUpdate.RequestedBy} <br />");
                sb.Append($"RequestDate: {accountRequestUpdate.RequestDate} <br />");
                sb.Append("</p></body></htm>");

            }

            string StatusStr = string.Empty;
            StatusStr = (RequestStatusList[accountRequestUpdate.RequestStatus].ToString().ToUpper() == "UNDER DIFFERENT GROUP" || RequestStatusList[accountRequestUpdate.RequestStatus].ToString().ToUpper() == "ACCOUNT CLOSED" ? "Modified" : RequestStatusList[accountRequestUpdate.RequestStatus].ToString());
            //await _emailSender.SendAccountRequestCreatedNotificationWithFile(accountRequestUpdate.RequestedBy, sb.ToString(), accountRequestUpdate.PracticeName, FileData, "AccountRequest.PDF");

            return Content(JsonConvert.SerializeObject("OK"));
        }
        public async Task<string> GeneratePDFAsync(int AccReqID)
        {
            var AccountRequest = await _wdb.AccountRequests.SingleOrDefaultAsync(m => m.AccountRequestID == AccReqID);

            AccountRequest.PhysicianRequests = await _wdb.PhysicianRequests.Where(m => m.AccountRequestID == AccReqID).ToListAsync();
            var fmodel = new AccountRequestViewModel();
            fmodel.accountRequest = AccountRequest;

            //Getting group details to get name to print in PDF
            string DeliveryOption = string.Empty;
            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            claim = from c in User.Claims where c.Type == "Group" select c.Value;
            var Groups = claim.ToList();
            var group = from g in _wdb.Groups
                        where Claims.Contains(g.SalesRep) || Claims.Contains(g.Manager) || Groups.Contains(g.GroupID.ToString()) || User.IsInRole("Phlebotomy Manager")
                        select g;
            var Group = group.ToList();

            var Result = _wdb.ResultDeliveryOptions.Where(t => t.AccountRequestID == AccReqID).ToList();

            if (Result.Count > 0)
            {
                foreach (var item in Result)
                {
                    DeliveryOption = DeliveryOption + ", " + Options[item.OptionID].ToString();
                }
            }
            fmodel.DeliveryMethod = DeliveryOption.Trim(',');
            fmodel.RequestStatus = "New"; //We are creating new request from this page so it will be always new
            fmodel.GroupName = Group.Where(x => x.GroupID == AccountRequest.GroupID).FirstOrDefault().Name;
            fmodel.ReqType = ReqTypeList[AccountRequest.RequestType].ToString();
            var testContent = await _templateService.RenderTemplateAsync("~/Views/NewAccountRequestPDF.cshtml", fmodel);
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
    }
}
