﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Bonus_Nonsense
    {
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        [Column(TypeName = "money")]
        public decimal? Cutoff { get; set; }
        [Column(TypeName = "money")]
        public decimal? Bonus { get; set; }
    }
}