﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Phleb_Expenses
    {
        [StringLength(50)]
        public string Employee { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public int? Account { get; set; }
        [StringLength(50)]
        public string Group { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        public int Key { get; set; }
        public bool? Type { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
    }
}