﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_EMR_Maintenance
    {
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public int? Account { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [StringLength(50)]
        public string vendor { get; set; }
        public bool? Auto { get; set; }
        public int Key { get; set; }
        public int? emrkey { get; set; }
    }
}