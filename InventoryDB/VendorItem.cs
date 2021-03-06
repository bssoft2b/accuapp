// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryDB
{
    public partial class VendorItem
    {
        public VendorItem()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
        }

        [Key]
        public int VendorItemID { get; set; }
        public int VendorID { get; set; }
        [Required]
        [StringLength(50)]
        public string ItemID { get; set; }
        public int VendorPriority { get; set; }
        [Required]
        [StringLength(50)]
        public string VendorItemIdentifier { get; set; }
        [Required]
        [StringLength(500)]
        public string VendorItemName { get; set; }
        public double QuantityMultiplier { get; set; }
        public double Cost { get; set; }
        public double TaxRate { get; set; }
        public int? AverageMonthlyUsage { get; set; }

        [ForeignKey(nameof(VendorID))]
        [InverseProperty("VendorItems")]
        public virtual Vendor Vendor { get; set; }
        [InverseProperty(nameof(PurchaseOrderLine.VendorItem))]
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
    }
}