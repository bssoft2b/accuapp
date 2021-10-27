﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryDB
{
    public partial class PurchaseOrderLine
    {
        [Key]
        public int PurchaseOrderLineID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int VendorItemID { get; set; }
        public int OrderedQty { get; set; }
        public double UnitCost { get; set; }
        public double TaxRate { get; set; }
        public int? ReceivedQty { get; set; }
        public int? CancelledQty { get; set; }
        public int? BackOrderQty { get; set; }

        [ForeignKey(nameof(PurchaseOrderID))]
        [InverseProperty("PurchaseOrderLines")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey(nameof(VendorItemID))]
        [InverseProperty("PurchaseOrderLines")]
        public virtual VendorItem VendorItem { get; set; }
    }
}