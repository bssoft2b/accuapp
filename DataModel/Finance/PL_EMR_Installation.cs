﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_EMR_Installation
    {
        public int? Account { get; set; }
        [Column("Vendor Name")]
        [StringLength(50)]
        public string Vendor_Name { get; set; }
        [Column("Live Date", TypeName = "date")]
        public DateTime? Live_Date { get; set; }
        [Column("comtron Installation", TypeName = "money")]
        public decimal? comtron_Installation { get; set; }
        [Column("vendor installation", TypeName = "money")]
        public decimal? vendor_installation { get; set; }
        [Column("comtron monthly", TypeName = "money")]
        public decimal? comtron_monthly { get; set; }
        [Column("vendor monthly", TypeName = "money")]
        public decimal? vendor_monthly { get; set; }
        public int key { get; set; }
        [Column("Comtron Termination Date", TypeName = "date")]
        public DateTime? Comtron_Termination_Date { get; set; }
        [Column("Vendor Termination Date", TypeName = "date")]
        public DateTime? Vendor_Termination_Date { get; set; }
        [Column("annual cost", TypeName = "money")]
        public decimal? annual_cost { get; set; }
        public bool cancel { get; set; }
    }
}