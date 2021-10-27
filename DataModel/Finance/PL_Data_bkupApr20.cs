﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Data_bkupApr20
    {
        [StringLength(50)]
        public string Category { get; set; }
        [StringLength(50)]
        public string SubCategory { get; set; }
        [StringLength(50)]
        public string Transaction { get; set; }
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        public int? Group { get; set; }
        [Column("Group Manager")]
        [StringLength(50)]
        public string Group_Manager { get; set; }
        public double? SubGroup { get; set; }
        public int? Account { get; set; }
        [Column("Account Name")]
        [StringLength(50)]
        public string Account_Name { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
        [Column("Chart Data", TypeName = "money")]
        public decimal? Chart_Data { get; set; }
        public int data_id { get; set; }
    }
}