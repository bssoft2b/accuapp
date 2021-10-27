﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Reagent_Costs
    {
        [Column("Client#")]
        public int? Client_ { get; set; }
        [Column("Client Name")]
        [StringLength(50)]
        public string Client_Name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column("Test#")]
        [StringLength(50)]
        public string Test_ { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? Number { get; set; }
        [Column("Test Cost", TypeName = "money")]
        public decimal? Test_Cost { get; set; }
        [Column("Total Cost", TypeName = "money")]
        public decimal? Total_Cost { get; set; }
        [Column("Test Type")]
        [StringLength(50)]
        public string Test_Type { get; set; }
        [Column("Physician #")]
        public int? Physician__ { get; set; }
    }
}