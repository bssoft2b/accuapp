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

namespace AccuApp32MVC.Controllers
{
    public class CodingController : Controller
    {
        private readonly WebdbContext _contextWebDb;
        private readonly WebdbContextProcedures _contextProcedureWebDb;

        private readonly PhlebotomyContext _contextPhlebotomy;

        private readonly UserManager<ApplicationUser> _userInManager;

        public CodingController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext contextPhlebotomy)
        {
            _contextWebDb = contextWebDb;
            _contextProcedureWebDb = contextProcedureWebDb;
            _contextPhlebotomy = contextPhlebotomy;
        }

        public IActionResult Index()
        {

            return View("CodingInvoicesList");
        }
        public IActionResult GetCodingInvoicesList()
        {
            ViewBag.StatusList = _contextWebDb.Coding_Codes.OrderBy(t => t.Code).ToList();
            ViewBag.TeamList= _contextWebDb.Coding_Activities.Select(x => x.User).Distinct().ToList();

            return View("CodingInvoicesList");
        }
        public IActionResult GetCodingInvoicesListJson()
        {
            //get parameters

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var invoice = Request.Form["invoice"].ToString();
            var status = Request.Form["status"].ToString();
            var btype = Request.Form["btype"].ToString();
            var pbtype = Request.Form["pbtype"].ToString();
            var user = Request.Form["user"].ToString();
            var fromdate = Request.Form["fromdate"].ToString();
            var todate = Request.Form["todate"].ToString();

            var a3 = _contextWebDb.Coding_Activities.Where(t => t.Table == "Coding_Invoices").GroupBy(t => t.PKey)
                .Select(t => new {
                        PKey = t.Key,
                        MaxDate =t.Max(l=>l.Date)
                    });
            var a4 = a3.GroupJoin(_contextWebDb.Coding_Activities, l1 => l1.PKey, r1 => r1.PKey, (l2, r2) => new { l2, r2 })
                .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                .Select(t => new
                {
                    Pkey = t.r4.PKey,
                    Date = t.r4.Date,
                    User = t.r4.User

                });
            var r = _contextWebDb.Coding_Invoices.GroupJoin(a4, l5 => Convert.ToString(l5.key), r5 => r5.Pkey, (l6, r6) => new { l6, r6 })
                .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 }).Where(t=>t.l8.l6.Active.HasValue && t.l8.l6.Active.Value)
                .Select(t=>new { 
                    Date=t.r8.Date??DateTime.Parse("01/01/2000"),
                    User=t.r8.User,
                    InvoiceNo=t.l8.l6.InvoiceNo,
                    BillType=t.l8.l6.BillType,
                    Status=t.l8.l6.Status,
                    Notes=t.l8.l6.Notes,
                    Key=t.l8.l6.key,
                    Active=t.l8.l6.Active,
                    PrevBillType=t.l8.l6.PrevBillType
                });


            var recordsTotal = r.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(invoice) || !string.IsNullOrEmpty(status) || !string.IsNullOrEmpty(btype) || !string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(pbtype) || !string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                //filtering
                if (!string.IsNullOrEmpty(invoice))
                    r = r.Where(t => t.InvoiceNo.Contains(invoice));

                if (!string.IsNullOrEmpty(status))
                    r = r.Where(t => t.Status.Contains(status));

                if (!string.IsNullOrEmpty(btype))
                    r = r.Where(t => Convert.ToString(t.BillType).Contains(btype));

                if (!string.IsNullOrEmpty(user))
                    r = r.Where(t => t.User==user);

                if (!string.IsNullOrEmpty(pbtype))
                    r = r.Where(t => t.PrevBillType.Contains(pbtype));

                if (!string.IsNullOrEmpty(fromdate))
                {
                    var fd = DateTime.Parse(fromdate);
                    r = r.Where(t => t.Date>=fd);
                }

                if (!string.IsNullOrEmpty(todate))
                {
                    var td = DateTime.Parse(todate).AddDays(1);
                    r = r.Where(t => t.Date<td);
                }


                recordsFiltered = r.Count();
            }
            //ordering

            if (orderColumn == 1)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.InvoiceNo) : r.OrderByDescending(t => t.InvoiceNo);
            }
            if (orderColumn == 2)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.PrevBillType) : r.OrderByDescending(t => t.PrevBillType);
            }
            if (orderColumn == 3)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.BillType) : r.OrderByDescending(t => t.BillType);
            }
            if (orderColumn == 4)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.Status) : r.OrderByDescending(t => t.Status);
            }
            if (orderColumn == 5)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.User) : r.OrderByDescending(t => t.User);
            }
            if (orderColumn == 7)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.Date) : r.OrderByDescending(t => t.Date);
            }

            var rList = r.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = rList,
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetInvoice(int? id)
        {
            var item = _contextWebDb.Coding_Invoices.FirstOrDefault(t => t.key == id);
            if (item == null) {
                item = new Coding_Invoice();
            }

            return Content(JsonConvert.SerializeObject(item));
        }
        public IActionResult SaveCodingInvoice(int? key,string invoice,int? billType,string prevBillType,string status,string notes)
        {
            var ci = _contextWebDb.Coding_Invoices.FirstOrDefault(t => t.key == key);

            if (ci != null)
            {
                ci.InvoiceNo = invoice;
                ci.BillType =billType.HasValue?billType.Value:0;
                ci.Notes = notes;
                ci.Status = status;
                ci.PrevBillType = prevBillType;

            } else
            {
                ci = new Coding_Invoice
                {
                    InvoiceNo = invoice,
                    BillType = billType.HasValue ? billType.Value : 0,
                    Notes = notes,
                    Status = status,
                    PrevBillType = prevBillType
                };
                _contextWebDb.Coding_Invoices.Add(ci);
            }

            _contextWebDb.SaveChanges();

            if (ci.key > 0)
            {
                //insert coding activity
                var ca = new Coding_Activity();
                ca.PKey = Convert.ToString(ci.key);
                ca.Table = "Coding_Invoices";
                ca.User = User.Identity.Name;
                ca.Date = DateTime.Now;
                ca.Change = "Edited contents of invoice: " +ci.Status;

                _contextWebDb.Coding_Activities.Add(ca);
            }

            _contextWebDb.SaveChanges();
            return Content(JsonConvert.SerializeObject("OK"));
        }
    }
}
