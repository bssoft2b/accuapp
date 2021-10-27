﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Overtime
    {
        [Required]
        [Column("Employee Id")]
        [StringLength(50)]
        public string Employee_Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Week { get; set; }
        [Column("Overtime")]
        public double? Overtime1 { get; set; }
    }
}