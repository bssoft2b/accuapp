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
    [Table("t_bonusCalculation_bkup290620")]
    public partial class t_bonusCalculation_bkup290620
    {
        public int RowID { get; set; }
        public string Sales_Rep { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MonthDate { get; set; }
        [Column(TypeName = "money")]
        public decimal? Payments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Expenses { get; set; }
        [Column(TypeName = "money")]
        public decimal? GrossProfit { get; set; }
        [Column(TypeName = "money")]
        public decimal? Compensation { get; set; }
        [Column(TypeName = "money")]
        public decimal? Bonus { get; set; }
        public int? Group { get; set; }
    }
}