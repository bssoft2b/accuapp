using InventoryDB;
using System.Collections.Generic;

namespace AccuApp32MVC.Models
{
    public class SalesOrderView
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; }
        public string BarCodeSvg { get; set; }
    }
}
