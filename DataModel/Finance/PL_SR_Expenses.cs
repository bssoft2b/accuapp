﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_SR_Expenses
    {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Key]
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
    }
}