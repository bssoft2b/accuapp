﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class WeeklyPhlebHours
    {
        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public double Hours { get; set; }
    }
}