﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Supplies_Custom
    {
        public int Account { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public int? Number { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [Key]
        public int id { get; set; }
    }
}