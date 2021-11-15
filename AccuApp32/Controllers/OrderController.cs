using AccuApp.Services;
using AccuApp32.Services;
using AccuApp32MVC.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using InventoryDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhlebotomyDB;
using Spire.Barcode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private readonly IConverter _pdfConverter;



        public OrderController(
            PhlebotomyContext contextPhlebotomy,
            WebdbContext wdb,
            IEmailSender emailSender,
            ITemplateService templateService,UserManager<IdentityUser> userManager,InventoryContext invDb, InventoryContextProcedures contextProcedureInvDb)
        {
            _ph = contextPhlebotomy;

            _invDb = invDb;
            _contextProcedureInvDb = contextProcedureInvDb;

            _templateService = templateService;
            _emailSender = emailSender;
            _userManager = userManager;
            _wdb = wdb;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAdressesJson(int? accountId)
        {
            var data = _invDb.DeliveryAddresses.Where(t => t.AccountID == accountId).OrderBy(t => t.DeliveryAddressName)
                        .Select(t => new
                        {
                            t.DeliveryAddressID,
                            t.DeliveryAddressName,
                            t.AccountID,
                            t.Address,
                            t.AddressFlag,
                            t.City,
                            t.State,
                            t.Suite,
                            t.Zip
                        }).ToList();

            return Content(JsonConvert.SerializeObject(data));
        }
        public IActionResult GetAdressJson(int? deliveryAddressID)
        {
            var t = _invDb.DeliveryAddresses.FirstOrDefault(t => t.DeliveryAddressID == deliveryAddressID);
            if (t!= null) { 
                        var dataSuccess=new
                         {
                             message="OK",
                             t.DeliveryAddressID,
                             t.DeliveryAddressName,
                             t.AccountID,
                             t.Address,
                             t.AddressFlag,
                             t.City,
                             t.State,
                             t.Suite,
                             t.Zip
                         };
                return Content(JsonConvert.SerializeObject(dataSuccess));
            }
            var dataError = new
            {
                message="ERROR"
            };
            return Content(JsonConvert.SerializeObject(dataError));
        }

        public JsonResult LoadItems(string filter)
        {
            filter = filter.Trim().ToUpper();
            var items = _invDb.Items.Where(x => x.Orderable);

            if (!string.IsNullOrEmpty(filter))
            {
                items = items.Where(x => x.ItemName.ToUpper().Contains(filter) || x.ItemDescription.ToUpper().Contains(filter));
            }

            var itemList = items.Take(20).ToList();

            var data = itemList.Select(t => new
            {
                id = t.ItemID,
                label = t.ItemName + " - " + t.ItemDescription + " - " + t.InStockQty.ToString(),
                value = t.ItemName + " - " + t.ItemDescription + " - " + t.InStockQty.ToString()
            });

            return new JsonResult(data);
        }
        public JsonResult GetItem(string itemId)
        {
            var t = _invDb.Items.FirstOrDefault(t => t.ItemID == itemId);

            return new JsonResult(t);
        }
        public IActionResult SupplyOrders()
        {
            var claim = from c in User.Claims where c.Type == "Account" select c.Value;
            var Claims = claim.ToList();
            var grpclaim = from c in User.Claims where c.Type == "Group" select c.Value;
            var GrpClaims = grpclaim.ToList();
            var accounts = from a in _invDb.Accounts where (Claims.Contains(a.AccountID.ToString()) || GrpClaims.Contains(a.GroupID.ToString())) select a;
            var Accounts = accounts.OrderBy(t=>t.AccountID).ToList(); //only non terminated account should populate

            var orderStatuses = _invDb.SalesOrderStatuses.OrderBy(t => t.SalesOrderStatusID).ToList();

            var deliveryMethods=_invDb.DeliveryMethods.OrderBy(t => t.DeliveryMethodID).ToList();

            var SalesRepList = _wdb.Groups.Select(x => new Commission { PaidTo = x.SalesRep }).Distinct().OrderBy(x => x.PaidTo).ToList();



            Dictionary<int, string> accs = new Dictionary<int, string>();
            foreach(var a in accounts)
            {
                accs.Add(a.AccountID,a.AccountID+" - "+a.Name);
            }


            Dictionary<int, string> ordStatuses = new Dictionary<int, string>();
            foreach (var ords in orderStatuses)
            {
                ordStatuses.Add(ords.SalesOrderStatusID, ords.SalesOrderStatusID + " - " + ords.StatusName);
            }

            Dictionary<int, string> dlvMethods = new Dictionary<int, string>();
            foreach (var dvm in deliveryMethods)
            {
                dlvMethods.Add(dvm.DeliveryMethodID, dvm.DeliveryMethodID.ToString() + " - " + dvm.DeliveryMethodName);
            }

            Dictionary<string,string> srlist = new Dictionary<string, string>();
            foreach (var sr in SalesRepList)
            {
                srlist.Add(sr.PaidTo,sr.PaidTo);
            }



            ViewBag.Accounts = accs;
            ViewBag.OrderStatuses = ordStatuses;
            ViewBag.DeliveryMethods = dlvMethods;
            ViewBag.SalesRepList = srlist;

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

            //var claim = from c in User.Claims where c.Type == "Account" select c.Value;
            //var Claims = claim.ToList();
            //var grpclaim = from c in User.Claims where c.Type == "Group" select c.Value;
            //var GrpClaims = claim.ToList();
            //var SalesOrders = _invDb.SalesOrders.Include(x => x.SalesOrderStatus).Include(x => x.DeliveryMethod).Include(a => a.Account).Where(a => Claims.Contains(a.AccountID.ToString()) || GrpClaims.Contains(a.Account.GroupID.ToString())).ToList() ?? new List<SalesOrder>();


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
        public JsonResult GetSupplyOrder(int salesOrderID)
        {

            var claim = from c in User.Claims where c.Type == "Account" select c.Value;
            var Claims = claim.ToList();
            var grpclaim = from c in User.Claims where c.Type == "Group" select c.Value;
            var GrpClaims = grpclaim.ToList();
            var accs = from a in _invDb.Accounts where (Claims.Contains(a.AccountID.ToString()) || GrpClaims.Contains(a.GroupID.ToString())) select a;
            var Accounts = accs.OrderBy(t => t.AccountID).ToList();

            var accIds = Accounts.Select(t => t.AccountID).ToArray();

            var order =_invDb.SalesOrders.FirstOrDefault(t=>t.SalesOrderID == salesOrderID);

            if (order != null)
            {
                var deliveryAddresses = _invDb.DeliveryAddresses.Where(t => t.AccountID == order.AccountID).OrderBy(t => t.DeliveryAddressName)
                .Select(t => new
                {
                    t.AccountID,
                    t.DeliveryAddressID,
                    t.DeliveryAddressName
                }).ToList();

                var accIds1=deliveryAddresses.Select(t=>t.AccountID).ToArray();

                var newAccs = accIds1.Except(accIds).ToArray();

                var newAccounts = _invDb.Accounts.Where(t => newAccs.Contains(t.AccountID)).ToList();

                Accounts.AddRange(newAccounts);

                var accounts = Accounts.OrderBy(t => t.AccountID).Select(t => new
                {
                    t.AccountID,
                    t.Name
                });

                var dvAddress = _invDb.DeliveryAddresses.FirstOrDefault(t => t.DeliveryAddressID == order.DeliveryAddressID);

                var deliveryAddress = new
                {
                    AccountID = dvAddress != null ? dvAddress.AccountID.ToString() : string.Empty,
                    Address = dvAddress != null ? dvAddress.Address : string.Empty,
                    AddressFlag = dvAddress != null ? dvAddress.AddressFlag : false,
                    City = dvAddress != null ? dvAddress.City : string.Empty,
                    Suite = dvAddress != null ? dvAddress.Suite : string.Empty,
                    DeliveryAddressID = dvAddress != null ? dvAddress.DeliveryAddressID : -1,
                    DeliveryAddressName = dvAddress != null ? dvAddress.DeliveryAddressName : string.Empty,
                    State = dvAddress != null ? dvAddress.State : string.Empty,
                    Zip = dvAddress != null ? dvAddress.Zip : string.Empty
                };

                var orderLines = _invDb.SalesOrderLines.Join(_invDb.Items,l=>l.ItemID,r=>r.ItemID,(l,r)=>new {l,r})
                    .Where(t => t.l.SalesOrderID == order.SalesOrderID).OrderBy(t => t.l.SalesOrderLineID)
                    .Select(t => new
                    {
                        t.l.SalesOrderID,
                        t.l.SalesOrderLineID,
                        t.l.OrderedQty,
                        t.l.BackOrderedQty,
                        t.l.CancelledQty,
                        t.l.CommittedQty,
                        t.l.UnitCost,
                        t.l.ItemID,
                        t.r.ItemName,
                        t.r.ItemDescription,
                        t.r.ItemCost
                    }).ToList();

                var data = new
                {
                    message="OK",
                    order.SalesOrderID,
                    order.SalesOrderStatusID,
                    order.AccountID,
                    order.BackOrderSalesOrderID,
                    order.CompletedDate,
                    order.CreatedBy,
                    order.DateCreated,
                    order.DatePrinted,
                    order.DateShipped,
                    order.DeliveryAddressID,
                    order.DeliveryMethodID,
                    order.OrderNote,
                    order.SalesRep,
                    order.ShippingNote,
                    order.TrackingNumber,
                    order.UpdatedBy,
                    order.UpdatedDate,
                    orderLines,
                    deliveryAddresses,
                    deliveryAddress,
                    accounts
                };
                return new JsonResult(data);
            }
            return new JsonResult(new { message="ERROR" });
        }
        public JsonResult SaveSupplyOrder(
                    int? supplyOrderID,
                    int? orderStatusID,
                    int? deliveryMethodID,
                    int? accountID,
                    int? deliveryAddressID,
                    string deliveryAddressName,
                    string address,
                    string city,
                    string suite,
                    string state,
                    string zip,
                    int[] orderLineIds,
                    string[] itemIds,
                    int[] orderedQty,
                    double[] unitCost,
                    int[] committedQty,
                    int[] cancelledQty,
                    int[] backOrderedQty,
                    string orderNote,
                    string shippingNote,
                    DateTime? dateShipped,
                    string salesRep,
                    string trackingNumber
            )
        {

            var supplyOrder = _invDb.SalesOrders.FirstOrDefault(t => t.SalesOrderID== supplyOrderID);

            if (supplyOrder == null)
                supplyOrder = new SalesOrder();

            //delivery address
            if (deliveryAddressID == -1 && accountID.HasValue)
            {
                //new delivery address
                var deliveryAddress = new DeliveryAddress
                {
                     AccountID=accountID.Value,
                     DeliveryAddressName = deliveryAddressName,
                     Address = address,
                     City=city,
                     Suite=suite,
                     State=state,
                     Zip=zip
                };
                _invDb.DeliveryAddresses.Add(deliveryAddress);
                _invDb.SaveChanges();

                supplyOrder.DeliveryAddressID = deliveryAddress.DeliveryAddressID;
            } else
            {
                supplyOrder.DeliveryAddressID = deliveryAddressID??0;
            }

            supplyOrder.AccountID = accountID.Value;
            supplyOrder.SalesRep = salesRep;
            supplyOrder.TrackingNumber = trackingNumber;
            supplyOrder.CompletedDate = (supplyOrder.SalesOrderID> 0 && orderStatusID.HasValue && orderStatusID.Value == 3 && supplyOrder.SalesOrderStatusID != 3) ? DateTime.Now : null;
            supplyOrder.CreatedBy = (supplyOrderID == null) ? User.Identity.Name : supplyOrder.CreatedBy;
            supplyOrder.DateCreated= (supplyOrderID == null) ? DateTime.Now: supplyOrder.DateCreated;
            supplyOrder.DateShipped = dateShipped;
            supplyOrder.DeliveryMethodID = deliveryMethodID;
            supplyOrder.OrderNote = orderNote;
            supplyOrder.SalesOrderStatusID = orderStatusID.HasValue? orderStatusID.Value: supplyOrder.SalesOrderStatusID;
            supplyOrder.ShippingNote = shippingNote;
            //supplyOrder.TrackingNumber=
            supplyOrder.UpdatedBy = (supplyOrderID != null) ? User.Identity.Name : supplyOrder.UpdatedBy;
            supplyOrder.UpdatedDate = (supplyOrderID != null) ? DateTime.Now : supplyOrder.UpdatedDate;

            //save header
            if (supplyOrderID == null)
                _invDb.SalesOrders.Add(supplyOrder);
            _invDb.SaveChanges();


            var oldOrderLinedIds= _invDb.SalesOrderLines.Where(t=>t.SalesOrderID==supplyOrderID).Select(t => t.SalesOrderLineID).ToArray();
            var delOrderLines = oldOrderLinedIds.Except(orderLineIds).ToArray();

            var delOrderLinesItem = _invDb.SalesOrderLines.Where(t => delOrderLines.Contains(t.SalesOrderLineID)).ToList();
            _invDb.SalesOrderLines.RemoveRange(delOrderLinesItem);

            for(var i=0;i< orderLineIds.Length; i++)
            {
                if (orderLineIds[i] == -1)
                {
                    var ordLine = new SalesOrderLine
                    {
                        SalesOrderID = supplyOrder.SalesOrderID,
                        BackOrderedQty = backOrderedQty[i],
                        OrderedQty = orderedQty[i],
                        CancelledQty = cancelledQty[i],
                        CommittedQty = committedQty[i],
                        UnitCost = unitCost[i],
                        ItemID=itemIds[i]
                    };
                    _invDb.SalesOrderLines.Add(ordLine);
                } else
                {
                    var ordLine = _invDb.SalesOrderLines.FirstOrDefault(t=>t.SalesOrderLineID== orderLineIds[i]);
                    if (ordLine!=null)
                    {
                        ordLine.SalesOrderID = supplyOrder.SalesOrderID;
                        ordLine.BackOrderedQty = backOrderedQty[i];
                        ordLine.OrderedQty = orderedQty[i];
                        ordLine.CancelledQty = cancelledQty[i];
                        ordLine.CommittedQty = committedQty[i];
                        ordLine.UnitCost = unitCost[i];
                        ordLine.ItemID=itemIds[i];
                    }
                }
            }
            _invDb.SaveChanges();

            return new JsonResult("OK");
        }

        public JsonResult DeleteSupplyOrder(int salesOrderID)
        {
            var salesOrderLines=_invDb.SalesOrderLines.Where(t=>t.SalesOrderID== salesOrderID).ToList();
            var salesOrder=_invDb.SalesOrders.FirstOrDefault(t=>t.SalesOrderID == salesOrderID);

            _invDb.SalesOrderLines.RemoveRange(salesOrderLines);
            _invDb.SalesOrders.Remove(salesOrder);

            _invDb.SaveChanges();

            return new JsonResult("OK");
        }

        public async Task<JsonResult> SalesOrderApprove(int salesOrderID)
        {

            try
            {
                var fmodel = new List<SalesOrderView>();
                var so = await _invDb.SalesOrders.Include(s => s.Account).Include(s => s.DeliveryAddress).Where(s => s.SalesOrderID == salesOrderID).ToListAsync();
                foreach (var item in so)
                {
                    if (item.SalesOrderStatusID != 3 && item.SalesOrderStatusID != 5)
                    {
                        item.SalesOrderStatusID = 3;
                        item.DatePrinted = DateTime.Now;
                        item.CompletedDate = DateTime.Now;
                        _invDb.SalesOrders.Attach(item).State =Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await _invDb.SaveChangesAsync();
                    }

                    SalesOrderView model = new SalesOrderView
                    {
                        SalesOrder = item,
                        SalesOrderLines = _invDb.SalesOrderLines.Where(l => l.SalesOrderID == item.SalesOrderID).ToList()
                    };

                    var barCodeSetting = new BarcodeSettings
                    {
                        AutoResize = true,
                        Type = BarCodeType.Code128,
                        Data = model.SalesOrder.SalesOrderID.ToString(),
                        TextColor = Color.White,
                        TopTextColor = Color.White,
                        ShowTopText = false
                    };
                    BarCodeGenerator bc = new BarCodeGenerator(barCodeSetting);
                    var img = bc.GenerateImage();


                    MemoryStream stream = new MemoryStream();
                    img.Save(stream, ImageFormat.Png);
                    byte[] imageBytes = stream.ToArray();
                    model.BarCodeSvg = Convert.ToBase64String(imageBytes);
                    fmodel.Add(model);
                }
                var testContent = await _templateService.RenderTemplateAsync("~/Views/PickTicket.cshtml", fmodel);
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
                var file = Convert.ToBase64String(output);
                //await _emailSender.SendPickTicketCopy(file);
            }
            catch (Exception)
            {
                throw;
            }
            return new JsonResult("Approved");

        }
        public async Task<JsonResult> PrintSalesOrder(int id)
        {
            //var user = await GetCurrentUserAsync();
            var fmodel = new List<SalesOrderView>();
            var so = await _invDb.SalesOrders.Include(s => s.Account).Include(s => s.DeliveryAddress).Where(s => s.SalesOrderID == id).ToListAsync();
            foreach (var item in so)
            {
                if (item.SalesOrderStatusID != 3 && item.SalesOrderStatusID != 5)
                {
                    item.SalesOrderStatusID = 4;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = User.Identity.Name;
                    item.DatePrinted = DateTime.Now;
                    _invDb.SalesOrders.Attach(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _invDb.SaveChangesAsync();
                }

                SalesOrderView model = new SalesOrderView
                {
                    SalesOrder = item,
                    //SalesOrderLines = _invDb.SalesOrderLines.Include(l => l.Item).Where(l => l.SalesOrderID == item.SalesOrderID).ToList()
                };

                var barCodeSetting = new BarcodeSettings
                {
                    AutoResize = true,
                    Type = BarCodeType.Code128,
                    Data = model.SalesOrder.SalesOrderID.ToString(),
                    TextColor = Color.White,
                    TopTextColor = Color.White,
                    ShowTopText = false
                };
                BarCodeGenerator bc = new BarCodeGenerator(barCodeSetting);
                var img = bc.GenerateImage();


                MemoryStream stream = new MemoryStream();
                img.Save(stream, ImageFormat.Png);
                byte[] imageBytes = stream.ToArray();
                model.BarCodeSvg = Convert.ToBase64String(imageBytes);
                fmodel.Add(model);
            }
            var testContent = await _templateService.RenderTemplateAsync("~/Views/PickTicket.cshtml", fmodel);
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
            return new JsonResult("{\"PDF\":\"" + file + "\"}");
        }

        public IActionResult PurchaseOrders()
        {
            var vendors=_invDb.Vendors.OrderBy(t=>t.VendorName).ToList();
            Dictionary<int, string> vds = new Dictionary<int, string>();
            foreach (var v in vendors)
            {
                vds.Add(v.VendorID, v.VendorID + " - " + v.VendorName);
            }

            ViewBag.Vendors = vds;  

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
                        t.l4.l2.DatePlaced,
                        t.l4.l2.DateFulfilled,
                        t.l4.l2.InvoiceDate,
                    });

            var recordsTotal = r.Count();

            if (!string.IsNullOrEmpty(searchValue))
            {
                r=r.Where(t=>Convert.ToString(t.PurchaseOrderID).Contains(searchValue) || t.VendorName.ToUpper().Contains(searchValue));
            }

            var recordsFiltered = recordsTotal;

            if (orderColumn == 0)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.PurchaseOrderID) : r.OrderByDescending(t => t.PurchaseOrderID);
            }
            if (orderColumn == 1)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.VendorName) : r.OrderByDescending(t => t.VendorName);
            }
            if (orderColumn == 2)
            {
                r = (orderDirect == "asc") ? r.OrderBy(t => t.DateCreated) : r.OrderByDescending(t => t.DateCreated);
            }

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
        public IActionResult GetPurchaseOrder(int purchaseOrderID)
        {
            var purchaseOrder=_invDb.PurchaseOrders.FirstOrDefault(t=>t.PurchaseOrderID == purchaseOrderID);

            if (purchaseOrder != null)
            {
                var orderLines = _invDb.PurchaseOrderLines.Join(_invDb.VendorItems, l => l.VendorItemID, r => r.VendorItemID, (l, r) => new { l, r })
                    .Where(t=>t.l.PurchaseOrderID== purchaseOrderID)
                    .Select(t => new
                    {
                        t.l.PurchaseOrderID,
                        t.l.PurchaseOrderLineID,
                        t.l.VendorItemID,
                        t.r.VendorItemName,
                        t.r.VendorItemIdentifier,
                        t.r.VendorPriority,
                        t.r.AverageMonthlyUsage,
                        t.r.Cost,
                        t.r.ItemID,
                        TaxRate=t.r.TaxRate,
                        t.r.QuantityMultiplier,
                        t.r.VendorID,
                        BackOrderQty=t.l.BackOrderQty??0,
                        CancelledQty=t.l.CancelledQty??0,
                        t.l.OrderedQty,
                        ReceivedQty=t.l.ReceivedQty??0,
                        t.l.UnitCost
                    });

                var data = new
                {
                    purchaseOrder.PurchaseOrderID,
                    purchaseOrder.BackOrderPurchaseOrderID,
                    purchaseOrder.DateCreated,
                    purchaseOrder.DateFulfilled,
                    purchaseOrder.DatePlaced,
                    purchaseOrder.InvoiceDate,
                    purchaseOrder.Memo,
                    purchaseOrder.OrderNote,
                    purchaseOrder.ShipTo,
                    purchaseOrder.VendorID,
                    orderLines 
                };
                return new JsonResult(data);
            }
            return new JsonResult("ERROR");
        }
        public IActionResult GetVendorsItems(int vendorID)
        {

            var vendorItems = _invDb.VendorItems.Where(t => t.VendorID == vendorID)
                .Join(_invDb.Items,l=>l.ItemID,r=>r.ItemID,(l,r)=>new {l,r })
                .OrderBy(t => t.l.VendorItemName)
                .Select(t=>new
                {
                    ItemID=t.l.VendorItemID,
                    Label=t.l.VendorItemID.ToString()+" - "+t.l.VendorItemName+" : "+t.r.InStockQty.ToString()+" in stock"
                })
                .ToList();
            return new JsonResult(vendorItems);
        }

        public IActionResult GetVendorItem(int vendorItemID)
        {
            var data= _invDb.VendorItems.Join(_invDb.Items, l => l.ItemID, r => r.ItemID, (l, r) => new {l,r})
                .Where(t=>t.l.VendorItemID==vendorItemID)
                .Select(t=>new
                {
                    t.l.VendorID,
                    t.l.VendorItemID,
                    t.l.VendorItemName,
                    t.l.VendorItemIdentifier,
                    t.l.VendorPriority,
                    t.l.Cost,
                    t.l.ItemID,
                    t.l.QuantityMultiplier,
                    t.l.TaxRate,
                    t.r.ItemCost,
                    t.r.ItemDescription,
                    t.r.ItemName,
                    t.r.InStockQty,
                    t.r.Lotted,
                    t.r.Orderable,
                    t.r.UnitOfMeasure,
                    t.r.Reagent,
                    t.r.ReorderQty,
                    t.r.ReorderThreshold
                }).FirstOrDefault();

            return new JsonResult(data);
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
