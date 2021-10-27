﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Index(nameof(AccountID), Name = "Finances_AccountID")]
    [Index(nameof(Category), Name = "Finances_Category")]
    [Index(nameof(Date), Name = "Finances_Date")]
    [Index(nameof(GroupID), Name = "Finances_GroupID")]
    [Index(nameof(SubCategory), Name = "Finances_SubCategory")]
    [Index(nameof(Transaction), Name = "Finances_Transaction")]
    public partial class Finance
    {
        [Key]
        public int FinanceID { get; set; }
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
        [Required]
        [StringLength(50)]
        public string SubCategory { get; set; }
        [Required]
        [StringLength(50)]
        public string Transaction { get; set; }
        [Required]
        [StringLength(50)]
        public string SalesRep { get; set; }
        public int? GroupID { get; set; }
        public double? SubGroup { get; set; }
        public int? AccountID { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        [StringLength(50)]
        public string Manager { get; set; }
        [StringLength(2)]
        public string State { get; set; }

        [ForeignKey(nameof(AccountID))]
        [InverseProperty("Finances")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(GroupID))]
        [InverseProperty("Finances")]
        public virtual Group Group { get; set; }
    }
}