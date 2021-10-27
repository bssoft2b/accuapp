﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryDB
{
    public partial class ReturnOrderLine
    {
        [Key]
        public int ReturnOrderLineID { get; set; }
        public int ReturnOrderID { get; set; }
        [Required]
        [StringLength(50)]
        public string ItemID { get; set; }
        public int Qty { get; set; }
        public double UnitCost { get; set; }

        [ForeignKey(nameof(ReturnOrderID))]
        [InverseProperty("ReturnOrderLines")]
        public virtual ReturnOrder ReturnOrder { get; set; }
    }
}