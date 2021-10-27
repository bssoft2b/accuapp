using AccuApp.Services;
using InventoryDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhlebotomyDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebDB;

namespace AccuApp32MVC.Controllers
{
    public class OrderController : Controller
    {

        private readonly WebdbContext _wdb;
        private readonly WebdbContextProcedures _contextProcedureWebDb;

        private readonly PhlebotomyContext _ph;

        private readonly InventoryContext _invDb;
        private readonly InventoryContextProcedures _contextProcedureInvDb;


        private readonly UserManager<IdentityUser> _userManager;


        private readonly IEmailSender _emailSender;
        private readonly ITemplateService _templateService;


        public OrderController(WebdbContext contextWebDb, WebdbContextProcedures contextProcedureWebDb, PhlebotomyContext contextPhlebotomy, IEmailSender emailSender,
            ITemplateService templateService,UserManager<IdentityUser> userManager,InventoryContext invDb, InventoryContextProcedures contextProcedureInvDb)
        {
            _wdb = contextWebDb;
            _contextProcedureWebDb = contextProcedureWebDb;
            _ph = contextPhlebotomy;

            _invDb = invDb;
            _contextProcedureInvDb = contextProcedureInvDb;

            _templateService = templateService;
            _emailSender = emailSender;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SupplyOrders()
        {
            return View("SupplyOrders");
        }
        public IActionResult SupplyOrdersJson(int? orderStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();


            //var r = _invDb.SalesOrders.Include(x => x.SalesOrderStatus).Include(x => x.DeliveryMethod);

            var r = _invDb.SalesOrders.GroupJoin(_invDb.SalesOrderStatuses, l1 => l1.SalesOrderStatusID, r1 => r1.SalesOrderStatusID, (l2, r2) => new { l2, r2 })
                    .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                    .GroupJoin(_invDb.DeliveryMethods, l5 => l5.l4.l2.DeliveryMethodID, r5 => r5.DeliveryMethodID, (l6, r6) => new { l6, r6 })
                    .SelectMany(l7 => l7.r6.DefaultIfEmpty(), (l8, r8) => new { l8, r8 })
                    .Select(t => new
                    {
                        t.l8.l6.l4.l2.AccountID,
                        t.l8.l6.l4.l2.DateCreated,
                        t.l8.l6.l4.l2.DatePrinted,
                        t.l8.l6.l4.l2.DateShipped,
                        t.l8.l6.l4.l2.DeliveryMethodID,
                        t.r8.DeliveryMethodName,
                        t.l8.l6.l4.l2.SalesOrderID,
                        t.l8.l6.l4.l2.SalesOrderStatusID,
                        t.l8.l6.r4.StatusName,
                        t.l8.l6.l4.l2.TrackingNumber,
                        t.l8.l6.l4.l2.CreatedBy

                    });

            if (orderStatus.HasValue)
                r = r.Where(t => t.SalesOrderStatusID == orderStatus.Value);

            var recordsTotal = r.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                //filtering
                if (!string.IsNullOrEmpty(searchValue))
                    r = r.Where(t => Convert.ToString(t.AccountID).Contains(searchValue) || Convert.ToString(t.SalesOrderID).Contains(searchValue));

                recordsFiltered = r.Count();
            }

            //ordering

            if (orderColumn == 1)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.SalesOrderID) : r.OrderByDescending(t => t.SalesOrderID);
            }
            if (orderColumn == 2)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.DateCreated) : r.OrderByDescending(t => t.DateCreated);
            }
            if (orderColumn == 3)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.DatePrinted) : r.OrderByDescending(t => t.DatePrinted);
            }
            if (orderColumn == 4)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.DateShipped) : r.OrderByDescending(t => t.DateShipped);
            }
            if (orderColumn == 5)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.AccountID) : r.OrderByDescending(t => t.AccountID);
            }
            if (orderColumn == 6)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.SalesOrderStatusID) : r.OrderByDescending(t => t.SalesOrderStatusID);
            }
            if (orderColumn == 7)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.DeliveryMethodName) : r.OrderByDescending(t => t.DeliveryMethodName);
            }
            if (orderColumn == 8)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.StatusName) : r.OrderByDescending(t => t.StatusName);
            }
            if (orderColumn == 9)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.CreatedBy) : r.OrderByDescending(t => t.CreatedBy);
            }

            var rList = r.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = rList.Select(t => new
                {
                    t.SalesOrderID,
                    t.AccountID,
                    t.TrackingNumber,
                    t.CreatedBy,
                    t.DateCreated,
                    t.DatePrinted,
                    t.DateShipped,
                    t.DeliveryMethodID,
                    t.DeliveryMethodName,
                    t.SalesOrderStatusID,
                    t.StatusName
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult PurchaseOrders()
        {
            return View("PurchaseOrders");
        }
        public IActionResult PurchaseOrdersJson(int? orderStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();


            //var r = _invDb.SalesOrders.Include(x => x.SalesOrderStatus).Include(x => x.DeliveryMethod);

            var r = _invDb.PurchaseOrders.GroupJoin(_invDb.Vendors, l1 => l1.VendorID, r1 => r1.VendorID, (l2, r2) => new { l2, r2 })
                    .SelectMany(l3 => l3.r2.DefaultIfEmpty(), (l4, r4) => new { l4, r4 })
                    .Select(t => new
                    {
                        t.l4.l2.PurchaseOrderID,
                        t.l4.l2.DateCreated,
                        t.l4.l2.VendorID,
                        t.r4.VendorName,
                        t.l4.l2.DatePlaced
                    });

            r = r.Where(t => !t.DatePlaced.HasValue);

            var recordsTotal = r.Count();
            var recordsFiltered = recordsTotal;

            var rList = r.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = rList.Select(t => new
                {
                    t.PurchaseOrderID,
                    t.DateCreated,
                    t.VendorID,
                    t.VendorName,
                    t.DatePlaced
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }

        public IActionResult ReturnOrders()
        {
            return View("returnorders");
        }
        public IActionResult ReturnOrdersJson(int? orderStatus)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();


            var claim = from c in User.Claims where c.Type == "SalesRep" select c.Value;
            var Claims = claim.ToList();
            var r = from a in _invDb.ReturnOrders where ((Claims.Contains(a.Account.Group.SalesRep)) || (Claims.Contains(a.Account.Group.Manager))) where a.DateCompleted == null orderby a.DateCreated select a;

            var recordsTotal = r.Count();
            var recordsFiltered = recordsTotal;

            var rList = r.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = rList.Select(t => new
                {
                    t.ReturnOrderID,
                    t.DateCreated,
                    t.AccountID
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult Items()
        {
            return View("items");
        }
        public IActionResult ItemsJson(int? filter)
        {

            var draw = Convert.ToInt32(Request.Form["draw"].ToString());
            var start = Convert.ToInt32(Request.Form["start"].ToString());
            var length = Convert.ToInt32(Request.Form["length"].ToString());
            var orderColumn = Convert.ToInt32(Request.Form["order[0][column]"].ToString());
            var orderDirect = Request.Form["order[0][dir]"].ToString();
            var searchValue = Request.Form["search[value]"].ToString();

            var r = _invDb.Items.Select(t=>t);

            if (filter.HasValue)
            {
                switch (filter.Value)
                {
                    case 2:
                        r = r.Where(x => x.Orderable);
                        break;
                    case 3:
                        r = r.Where(x => !x.Orderable);
                        break;
                }
            }

            var recordsTotal = r.Count();
            var recordsFiltered = recordsTotal;

            if (!string.IsNullOrEmpty(searchValue))
            {
                r = r.Where(i=>(Convert.ToString(i.ItemID).Contains(searchValue)) ||
                    (i.ItemName == null || i.ItemName == "" ? false : i.ItemName.ToLower().Contains(searchValue.ToLower())) ||
                    (i.ItemDescription == null || i.ItemDescription == "" ? false : i.ItemDescription.ToLower().Contains(searchValue.ToLower())) ||
                    (Convert.ToString(i.InStockQty).Contains(searchValue)));

                recordsFiltered = r.Count();
            }

            //ordering

            if (orderColumn == 0)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.ItemID) : r.OrderByDescending(t => t.ItemID);
            }
            if (orderColumn == 1)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.Orderable) : r.OrderByDescending(t => t.Orderable);
            }
            if (orderColumn == 2)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.ItemName) : r.OrderByDescending(t => t.ItemName);
            }
            if (orderColumn == 3)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.ItemDescription) : r.OrderByDescending(t => t.ItemDescription);
            }
            if (orderColumn == 4)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.InStockQty) : r.OrderByDescending(t => t.InStockQty);
            }
            if (orderColumn == 5)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.ItemCost) : r.OrderByDescending(t => t.ItemCost);
            }

            var rList = r.Skip(start).Take(length).ToList();

            var data = new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = rList.Select(t => new
                {
                    t.ItemID,
                    t.Orderable,
                    t.ItemName,
                    t.ItemDescription,
                    t.InStockQty,
                    t.ItemCost
                }).ToList()
            };

            return Content(JsonConvert.SerializeObject(data));
        }
    }
}
