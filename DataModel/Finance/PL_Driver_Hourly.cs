﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Driver_Hourly
    {
        [StringLength(255)]
        public string Driver { get; set; }
        public int? Employee { get; set; }
        [Column(TypeName = "money")]
        public decimal? Rate { get; set; }
    }
}