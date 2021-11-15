using System.Collections.Generic;
using InventoryDB;

namespace AccuApp32MVC.Models
{
    public class LabSalesOrderView
    {
        public LabSalesOrder SalesOrder { get; set; }
        public List<LabSalesOrderLine> SalesOrderLines { get; set; }
        public string BarCodeSvg { get; set; }
    }

}
