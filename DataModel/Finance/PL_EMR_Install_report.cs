// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_EMR_Install_report
    {
        public int? Account { get; set; }
        [Column("Account Name")]
        [StringLength(255)]
        public string Account_Name { get; set; }
        [Column(TypeName = "decimal(10, 3)")]
        public decimal? Subgroup { get; set; }
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        [Column("Group Manager")]
        [StringLength(50)]
        public string Group_Manager { get; set; }
        [Column("Vendor name")]
        [StringLength(50)]
        public string Vendor_name { get; set; }
        [Column("Live Date", TypeName = "date")]
        public DateTime? Live_Date { get; set; }
        [Column("comtron termination Date", TypeName = "date")]
        public DateTime? comtron_termination_Date { get; set; }
        [Column("vendor termination Date", TypeName = "date")]
        public DateTime? vendor_termination_Date { get; set; }
        [Column("comtron Installation", TypeName = "money")]
        public decimal? comtron_Installation { get; set; }
        [Column("Vendor installation", TypeName = "money")]
        public decimal? Vendor_installation { get; set; }
        [Column("Comtron Monthly", TypeName = "money")]
        public decimal? Comtron_Monthly { get; set; }
        [Column("Vendor Monthly", TypeName = "money")]
        public decimal? Vendor_Monthly { get; set; }
        [Column("Annual Cost", TypeName = "money")]
        public decimal? Annual_Cost { get; set; }
        public int? Specimens { get; set; }
        public int? Status { get; set; }
        public int? key { get; set; }
        public bool? cancel { get; set; }
    }
}