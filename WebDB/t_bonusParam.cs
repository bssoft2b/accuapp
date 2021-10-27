﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Keyless]
    public partial class t_bonusParam
    {
        public int RowID { get; set; }
        public string SalesRep { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MonthDate { get; set; }
        [Column(TypeName = "money")]
        public decimal? Payments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Courier { get; set; }
        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
        [Column(TypeName = "money")]
        public decimal? FloatExpenses { get; set; }
        [Column(TypeName = "money")]
        public decimal? FloatPayroll { get; set; }
        [Column(TypeName = "money")]
        public decimal? PhlebExpenses { get; set; }
        [Column(TypeName = "money")]
        public decimal? PhlebPayroll { get; set; }
        [Column(TypeName = "money")]
        public decimal? EMRCost { get; set; }
        [Column(TypeName = "money")]
        public decimal? ReagentCost { get; set; }
        [Column(TypeName = "money")]
        public decimal? Supplies { get; set; }
        [Column(TypeName = "money")]
        public decimal? Rent { get; set; }
        [Column(TypeName = "money")]
        public decimal? Utilities { get; set; }
        [Column(TypeName = "money")]
        public decimal? RepsSalary { get; set; }
        public int? Group { get; set; }
    }
}