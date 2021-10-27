﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PhlebotomyDB
{
    [Table("PhlebotomistAssignmentLog")]
    public partial class PhlebotomistAssignmentLog
    {
        [Key]
        public int PhlebotomistAssignmentLogID { get; set; }
        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }
        [Column(TypeName = "date")]
        public DateTime Month { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastModifiedDate { get; set; }
        [Required]
        [StringLength(256)]
        public string LastModifiedBy { get; set; }
        public int? AccountID { get; set; }
        public int? GroupID { get; set; }
        public double Percentage { get; set; }
    }
}