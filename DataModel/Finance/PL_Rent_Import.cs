﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Rent_Import
    {
        public int? Account { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [StringLength(255)]
        public string address { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(255)]
        public string memo { get; set; }
        public bool? ignored { get; set; }
    }
}