﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryDB
{
    public partial class SalesOrderStatus
    {
        public SalesOrderStatus()
        {
            SalesOrders = new HashSet<SalesOrder>();
        }

        [Key]
        public int SalesOrderStatusID { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }
        [Required]
        [StringLength(250)]
        public string StatusDescription { get; set; }

        [InverseProperty(nameof(SalesOrder.SalesOrderStatus))]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}