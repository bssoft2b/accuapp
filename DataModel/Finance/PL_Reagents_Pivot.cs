﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Reagents_Pivot
    {
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        public int? Group { get; set; }
        [Column(TypeName = "decimal(10, 3)")]
        public decimal? SubGroup { get; set; }
        public int? Account { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [Column("Physician #")]
        public int? Physician__ { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column("Test Type")]
        [StringLength(50)]
        public string Test_Type { get; set; }
        [Column("Test Code")]
        [StringLength(50)]
        public string Test_Code { get; set; }
        [Column("Test Name")]
        [StringLength(50)]
        public string Test_Name { get; set; }
        public int? Count { get; set; }
        [Column("test cost", TypeName = "money")]
        public decimal? test_cost { get; set; }
        [Column("total cost", TypeName = "money")]
        public decimal? total_cost { get; set; }
    }
}