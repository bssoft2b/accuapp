﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class BillType
    {
        public BillType()
        {
            InvoiceBillTypes = new HashSet<Invoice>();
            InvoiceInsurance1s = new HashSet<Invoice>();
            InvoiceInsurance2s = new HashSet<Invoice>();
        }

        [Key]
        public int BillTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string BillTypeName { get; set; }
        public int BillingGroupID { get; set; }
        public bool InNetwork { get; set; }
        public int? TimelyFiling { get; set; }
        public bool Alertable { get; set; }
        public int? AlertCycle { get; set; }
        public bool Accepted { get; set; }
        [Required]
        public string Notes { get; set; }

        [ForeignKey(nameof(BillingGroupID))]
        [InverseProperty("BillTypes")]
        public virtual BillingGroup BillingGroup { get; set; }
        [InverseProperty(nameof(Invoice.BillType))]
        public virtual ICollection<Invoice> InvoiceBillTypes { get; set; }
        [InverseProperty(nameof(Invoice.Insurance1))]
        public virtual ICollection<Invoice> InvoiceInsurance1s { get; set; }
        [InverseProperty(nameof(Invoice.Insurance2))]
        public virtual ICollection<Invoice> InvoiceInsurance2s { get; set; }
    }
}