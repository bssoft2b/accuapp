﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryDB
{
    public partial class Account
    {
        public Account()
        {
            DeliveryAddresses = new HashSet<DeliveryAddress>();
            ReturnOrders = new HashSet<ReturnOrder>();
            SalesOrders = new HashSet<SalesOrder>();
        }

        [Key]
        public int AccountID { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int GroupID { get; set; }
        public double SubGroup { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Suite { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(10)]
        public string State { get; set; }
        [Required]
        [StringLength(10)]
        public string Zip { get; set; }
        [StringLength(15)]
        public string Telephone { get; set; }
        public bool Active { get; set; }
        public bool Profitable { get; set; }
        public int Specimens { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TerminationDate { get; set; }

        [ForeignKey(nameof(GroupID))]
        [InverseProperty("Accounts")]
        public virtual Group Group { get; set; }
        [InverseProperty(nameof(DeliveryAddress.Account))]
        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
        [InverseProperty(nameof(ReturnOrder.Account))]
        public virtual ICollection<ReturnOrder> ReturnOrders { get; set; }
        [InverseProperty(nameof(SalesOrder.Account))]
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}