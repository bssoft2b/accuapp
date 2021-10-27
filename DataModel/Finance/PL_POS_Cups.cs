﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_POS_Cups
    {
        [Key]
        public int Account { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public int? Number { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        public int id { get; set; }
        [Column("Transaction Number")]
        [StringLength(50)]
        public string Transaction_Number { get; set; }
        [Column("Payment Method")]
        [StringLength(50)]
        public string Payment_Method { get; set; }
    }
}