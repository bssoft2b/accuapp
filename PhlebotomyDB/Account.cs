﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PhlebotomyDB
{
    public partial class Account
    {
        public Account()
        {
            FloatStops = new HashSet<FloatStop>();
            PhlebotomistAssignmentLines = new HashSet<PhlebotomistAssignmentLine>();
            PhlebotomistExpenses = new HashSet<PhlebotomistExpense>();
        }

        [Key]
        public int AccountID { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int GroupID { get; set; }
        public double? SubGroup { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Suite { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(9)]
        public string Zip { get; set; }
        [StringLength(15)]
        public string Telephone { get; set; }
        [StringLength(15)]
        public string Fax { get; set; }
        public bool? Active { get; set; }
        [StringLength(20)]
        public string NPI { get; set; }

        [ForeignKey(nameof(GroupID))]
        [InverseProperty("Accounts")]
        public virtual Group Group { get; set; }
        [InverseProperty(nameof(FloatStop.Account))]
        public virtual ICollection<FloatStop> FloatStops { get; set; }
        [InverseProperty(nameof(PhlebotomistAssignmentLine.Account))]
        public virtual ICollection<PhlebotomistAssignmentLine> PhlebotomistAssignmentLines { get; set; }
        [InverseProperty(nameof(PhlebotomistExpense.Account))]
        public virtual ICollection<PhlebotomistExpense> PhlebotomistExpenses { get; set; }
    }
}