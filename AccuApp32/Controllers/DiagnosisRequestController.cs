using AccuApp32MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using PhlebotomyDB;
using DinkToPdf;
using DinkToPdf.Contracts;
using AccuApp.Services;

namespace AccuApp32MVC.Controllers
{
    public class DiagnosisRequestController : Controller
    {
        private readonly WebdbContext _wdb;
        private readonly PhlebotomyContext _ph;
        private readonly IConverter _pdfConverter;
        private readonly ITemplateService _templateService;



        private readonly WebdbContextProcedures _contextProcedureWebDb;
        private readonly UserManager<ApplicationUser> _userInManager;

        private int DROP_DOWN_COUNT = 100;

        public DiagnosisRequestController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext ph, IConverter pdfConverter, ITemplateService templateService)
        {
            _wdb = contextWebDb;
            _ph = ph;
            _contextProcedureWebDb = contextProcedureWebDb;
            _pdfConverter = pdfConverter;
            _templateService = templateService;

        }

        public IActionResult Index()
        {
            return View("DiagnosisRequest");
        }
        public IActionResult DiagnosisRequestListJson()
        {
            return null;
        }
        public IActionResult GetAccountDiagnosisRequest(int accountID)
        {
            var Account = _wdb.Accounts.FirstOrDefault(x => x.AccountID == accountID);

            var DiagnosisRequest = _wdb.DiagnosisRequests.Where(t => t.AccountID == accountID && t.DateSigned == null && t.DateCompleted == null).Join(_wdb.PatientDemographics, l => l.AccessionNumber, r => r.AccessionNumber, (l, r) => new { l, r })
                .Select(x => new
                {
                    x.l.AccountID,
                    x.l.DiagnosisRequestID,
                    x.l.PhysicianID,
                    x.l.AccessionNumber,
                    x.r.FirstName,
                    x.r.LastName,
                    x.r.DOB,
                    x.l.RequestDate,
                    LastPrinted=x.l.LastPrinted.HasValue? x.l.LastPrinted.Value.ToString("YYYY/DD/MM HH:mm"):"",
                    LastPrintedBy=x.l.LastPrintedBy??""
                }).ToList();

            var data = new
            {
                Account,
                DiagnosisRequest
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetAccountDiagonsisRequest(int diagnosisRequestID)
        {
            var Claims = User.Claims.Where(c => c.Type == "SalesRep").Select(c=>c.Value).ToList();
            var CommonDiagnosisCodes = _wdb.CommonDiagnosisCodes.Take(DROP_DOWN_COUNT).ToList();
            var Accounts=_wdb.Accounts.Where(a => (Claims.Contains(a.Group.SalesRep)) || (Claims.Contains(a.Group.Manager))).ToList();
            var DiagnosisCodes = _wdb.DiagnosisCodes.Take(DROP_DOWN_COUNT).ToList();

            var diagnosisRequest = _wdb.DiagnosisRequests.Join(_wdb.Accounts, dr => dr.AccountID, a => a.AccountID, (dr, a) => new { dr, a })
                .Join(_wdb.Accounts, dra => dra.dr.AccountID, a => a.AccountID, (dra, a) => new { dra, a })
                .Join(_wdb.PatientDemographics, draa => draa.dra.dr.AccessionNumber, pd => pd.AccessionNumber, (draa, pd) => new { draa, pd })
                .Join(_wdb.DiagnosisHistoryLines, draapd => draapd.draa.dra.dr.DiagnosisRequestID, dhl => dhl.DiagnosisRequestID, (draapd, dhl) => new { draapd, dhl })
                .FirstOrDefault(t => t.draapd.draa.dra.dr.DiagnosisRequestID == diagnosisRequestID);

            var diagnosisRequestLines = _wdb.DiagnosisRequestLines.Where(x => x.DiagnosisRequestID == diagnosisRequestID).ToList();

            var data = new
            {
                DiagnosisRequestID = diagnosisRequestID,
                CommonDiagnosisCodes=CommonDiagnosisCodes,
                Accounts=Accounts,
                DiagnosisCodes=DiagnosisCodes,
                DiagnosisRequest= diagnosisRequest,
                DiagnosisRequestLines= diagnosisRequestLines
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetDiagnosisRequestLines(int diagnosisRequestID)
        {
            var DiagnosisRequest = _wdb.DiagnosisRequests.Include(x => x.Account).Include(x => x.Physician).Include(x => x.PatientDemographic).Include(x => x.DiagnosisHistoryLines).FirstOrDefault(x => x.DiagnosisRequestID == diagnosisRequestID);

            var sDate = _wdb.Charges.FirstOrDefault(x => x.InvoiceID == DiagnosisRequest.AccessionNumber);
            var ServiceDate = sDate != null? sDate.Date.ToString("MM/dd/yyyy"):string.Empty;

            var DiagnosisLines = _wdb.DiagnosisLines.Where(x => x.DiagnosisRequestID == diagnosisRequestID).ToListAsync().Result;
            var DiagnosisCodes = _wdb.DiagnosisCodes.Take(200).ToListAsync().Result;
            var DiagnosisRequestLines = _wdb.DiagnosisRequestLines.Where(x => x.DiagnosisRequestID == DiagnosisRequest.DiagnosisRequestID).ToListAsync().Result;

            var data = new
            {
                DiagnosisRequestID = DiagnosisRequest.DiagnosisRequestID,
                AccountID = DiagnosisRequest.AccountID,
                Name1 = DiagnosisRequest.PhysicianID == 0 ? DiagnosisRequest.Account.Name : DiagnosisRequest.Physician.Name,
                Address = DiagnosisRequest.Account.Address,
                Telephone = DiagnosisRequest.Account.Telephone,
                PhysicianID = DiagnosisRequest.PhysicianID == 0 ? DiagnosisRequest.AccountID : DiagnosisRequest.PhysicianID,
                Name2 = DiagnosisRequest.PhysicianID == 0 ? DiagnosisRequest.Account.Name : DiagnosisRequest.Physician.Name,
                NPI = DiagnosisRequest.PhysicianID == 0 ? DiagnosisRequest.Account.NPI:DiagnosisRequest.Physician.NPI,
                AccessionNumber = DiagnosisRequest.AccessionNumber,
                ServiceDate = ServiceDate,
                RequestDate = DiagnosisRequest.RequestDate,
                FirstName = DiagnosisRequest.PatientDemographic.FirstName,
                LastName = DiagnosisRequest.PatientDemographic.LastName,
                DOB = DiagnosisRequest.PatientDemographic.DOB,
                DiagnosisHistoryLines= DiagnosisRequest.DiagnosisHistoryLines.Select(k=>new { 
                    k.DiagnosisRequestID,
                    k.DiagnosisHistoryLineID,
                    k.Code,
                    k.Description
                }),
                DiagnosisLines,
                DiagnosisCodes,
                DiagnosisRequestLines
            };
            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetCodeListJson(string searchValue)
        {
            var DiagnosisCodes = _wdb.DiagnosisCodes.Where(t=>t.Code.ToLower().Contains(searchValue) || t.Description.ToLower().Contains(searchValue)).OrderBy(t=>t.Code).Take(200).ToListAsync().Result;
            return Content(JsonConvert.SerializeObject(DiagnosisCodes));
        }
        public IActionResult GetAccountsListJson()
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var Claims = (from c in User.Claims where c.Type == "Account" select c.Value).ToList();
            var claim2 = (from c in User.Claims where c.Type == "PhlebotomyLead" select c.Value).ToList();

            Claims = Claims.Concat((from x in _ph.Phlebotomists
                                    join l in _ph.PhlebotomistLeads on x.EmployeeID equals l.EmployeeID
                                    join pa in _ph.PhlebotomistAssignments on x.EmployeeID equals pa.EmployeeID
                                    join pal in _ph.PhlebotomistAssignmentLines on pa.PhlebotomistAssignmentID equals pal.PhlebotomistAssignmentID
                                    select new { pal,pa,l}
                                    ).AsEnumerable()
                                    .Where(l=>claim2.Contains(Convert.ToString(l.l.LeadID)) && l.pal.AccountID.HasValue && l.pa.Month == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                                    .Select(l=>Convert.ToString(l.pal.AccountID))).ToList();

            var grpclaim = from c in User.Claims where c.Type == "Group" select c.Value;

            var GrpClaims = grpclaim.ToList();
            var grpclaim2 = (from c in User.Claims where c.Type == "PhlebotomyLead" select c.Value).ToList();
            GrpClaims = GrpClaims.Concat((from x in _ph.Phlebotomists
                                          join l in _ph.PhlebotomistLeads on x.EmployeeID equals l.EmployeeID
                                          join pa in _ph.PhlebotomistAssignments on x.EmployeeID equals pa.EmployeeID
                                          join pal in _ph.PhlebotomistAssignmentLines on pa.PhlebotomistAssignmentID equals pal.PhlebotomistAssignmentID
                                          select new {pal,pa,l }
                                          ).AsEnumerable()
                                          .Where(l=> claim2.Contains(Convert.ToString(l.l.LeadID)) && l.pal.GroupID.HasValue && l.pa.Month == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                                          .Select(l=>Convert.ToString(l.pal.GroupID))).ToList();


            var recordsTotal = 0;
            var recordsFiltered = 0;


            var _accounts = from d in _wdb.DiagnosisRequests
                            join a in _wdb.Accounts on d.AccountID equals a.AccountID
                            where d.DateSigned == null && d.DateCompleted == null && ((Claims.Contains(a.AccountID.ToString())) || (GrpClaims.Contains(a.GroupID.ToString())) || User.IsInRole("Phlebotomy Manager"))
                            select new
                            {
                                ac = a,
                                dr = d
                            };
            recordsTotal = _accounts.Select(x=>x.ac).Distinct().Count();
            recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                //check for nulls and only compare if it has data or else it will throw an error
                _accounts = _accounts.Where(a => (a.ac.Name.ToLower().Contains(searchValue.ToLower())) ||
                (a.ac.AccountID.ToString() == searchValue) || (a.dr.AccessionNumber.ToLower().Contains(searchValue.ToLower())));

                recordsFiltered = _accounts.Count();

            }

            var accounts = _accounts.Select(x => x.ac).Distinct();

            var DiagnosisRequests = _accounts.Select(x => x.dr).ToList();


            if (orderColumn == 0)
            {
                accounts = (orderDirect == "asc") ? accounts.OrderBy(t => t.AccountID) : accounts.OrderByDescending(t => t.AccountID);
            }

            if (orderColumn == 1)
            {
                accounts = (orderDirect == "asc") ? accounts.OrderBy(t => t.Name) : accounts.OrderByDescending(t => t.Name);
            }

            if (orderColumn == 2)
            {
                accounts = (orderDirect == "asc") ? accounts.OrderBy(t => t.GroupID) : accounts.OrderByDescending(t => t.GroupID);
            }

            if (orderColumn == 3)
            {
                accounts = (orderDirect == "asc") ? accounts.OrderBy(t => t.SubGroup) : accounts.OrderByDescending(t => t.SubGroup);
            }

            if (orderColumn == 4)
            {
                accounts = (orderDirect == "asc") ? accounts.OrderBy(t => t.Telephone) : accounts.OrderByDescending(t => t.Telephone);
            }

            var accountsList = accounts.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = accountsList.Select(t => new {
                    t.AccountID,
                    t.Name,
                    t.GroupID,
                    t.SubGroup,
                    t.Telephone,
                    OpenCount = DiagnosisRequests.Count(x=>x.AccountID==t.AccountID)
                }).ToList()
            };
            return Content(JsonConvert.SerializeObject(data));

        }
        async public Task<IActionResult> SaveDiagnosisReuestLines(int DiagnosisRequestID,string[] Codes, string esign)
        {

            var DiagnosisRequest = _wdb.DiagnosisRequests.Include(r => r.DiagnosisHistoryLines).FirstOrDefault(x => x.DiagnosisRequestID == DiagnosisRequestID);

            DiagnosisRequest.DateCompleted = DateTime.Now;
            DiagnosisRequest.DateSigned = DateTime.Now;
            DiagnosisRequest.Signature = esign;
            DiagnosisRequest.SignedBy = HttpContext.User.Identity.Name;
            _wdb.DiagnosisRequests.Attach(DiagnosisRequest).State = EntityState.Modified;

            await _wdb.SaveChangesAsync();

            var dls = Codes.Select(t => new DiagnosisLine
            {
                DiagnosisCode = t,
                DiagnosisRequestID = DiagnosisRequestID,
            }).ToList();

            try
            {
                await _wdb.DiagnosisLines.AddRangeAsync(dls);
                await _wdb.SaveChangesAsync();
            }catch(Exception ex)
            {
            }

            var DiagnosisRequestLines = await _wdb.DiagnosisRequestLines.Where(x => x.DiagnosisRequestID == DiagnosisRequest.DiagnosisRequestID).ToListAsync();
            DiagnosisRequest.Physician = await _wdb.Accounts.SingleOrDefaultAsync(x => x.AccountID == DiagnosisRequest.PhysicianID);
            DiagnosisRequest.Account = await _wdb.Accounts.SingleOrDefaultAsync(x => x.AccountID == DiagnosisRequest.AccountID);
            DiagnosisRequest.PatientDemographic = await _wdb.PatientDemographics.SingleOrDefaultAsync(x => x.AccessionNumber == DiagnosisRequest.AccessionNumber);

            var DiagnosisLines = _wdb.DiagnosisLines.Where(t => t.DiagnosisRequestID == DiagnosisRequestID).ToList();

            var View = new DiagnosisRequestsView
            {
                DiagnosisRequest = DiagnosisRequest,
                DiagnosisRequestLines = DiagnosisRequestLines,
                DiagnosisLines = DiagnosisLines
            };

            var testContent = await _templateService.RenderTemplateAsync("~/Views/CompletedDiagnosisRequest.cshtml", View);
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
                                HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                            }
                    }
            };

            var output = _pdfConverter.Convert(doc);
            var PageForPrint = Convert.ToBase64String(output);
            //await _wdb.Database.ExecuteSqlCommandAsync("sp_complete_dx_request @p0, @p1", parameters: new[] { DiagnosisRequest.AccessionNumber, PageForPrint });
            await _wdb.Database.ExecuteSqlRawAsync("sp_complete_dx_request @p0, @p1", parameters: new[] { DiagnosisRequest.AccessionNumber, PageForPrint });
            await _wdb.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = DiagnosisRequest.AccountID });
        }
    }
}
