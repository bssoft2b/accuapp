﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class t_Overhead_Extras
    {
        public int? Account { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        public string Notes { get; set; }
    }
}