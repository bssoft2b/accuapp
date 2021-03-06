// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PhlebotomyDB
{
    public partial class Group
    {
        public Group()
        {
            Accounts = new HashSet<Account>();
            PhlebotomistAssignmentLines = new HashSet<PhlebotomistAssignmentLine>();
            PhlebotomistExpenses = new HashSet<PhlebotomistExpense>();
        }

        [Key]
        public int GroupID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(15)]
        public string Telephone { get; set; }
        [Required]
        [StringLength(50)]
        public string SalesRep { get; set; }
        [Required]
        [StringLength(50)]
        public string Manager { get; set; }
        public bool Active { get; set; }
        public bool Negative { get; set; }

        [InverseProperty(nameof(Account.Group))]
        public virtual ICollection<Account> Accounts { get; set; }
        [InverseProperty(nameof(PhlebotomistAssignmentLine.Group))]
        public virtual ICollection<PhlebotomistAssignmentLine> PhlebotomistAssignmentLines { get; set; }
        [InverseProperty(nameof(PhlebotomistExpense.Group))]
        public virtual ICollection<PhlebotomistExpense> PhlebotomistExpenses { get; set; }
    }
}