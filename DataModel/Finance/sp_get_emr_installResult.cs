﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class sp_get_emr_installResult
    {
        public int? Account { get; set; }
        [Column("Account Name")]
        public string AccountName { get; set; }
        public decimal? Subgroup { get; set; }
        [Column("Sales Rep")]
        public string SalesRep { get; set; }
        [Column("Group Manager")]
        public string GroupManager { get; set; }
        [Column("Vendor name")]
        public string Vendorname { get; set; }
        [Column("Live Date")]
        public DateTime? LiveDate { get; set; }
        [Column("comtron termination Date")]
        public DateTime? comtronterminationDate { get; set; }
        [Column("vendor termination Date")]
        public DateTime? vendorterminationDate { get; set; }
        [Column("comtron Installation")]
        public decimal? comtronInstallation { get; set; }
        [Column("Vendor installation")]
        public decimal? Vendorinstallation { get; set; }
        [Column("Comtron Monthly")]
        public decimal? ComtronMonthly { get; set; }
        [Column("Vendor Monthly")]
        public decimal? VendorMonthly { get; set; }
        [Column("Annual Cost")]
        public decimal? AnnualCost { get; set; }
        public int? Specimens { get; set; }
        public int? Status { get; set; }
        public int? key { get; set; }
        public bool? cancel { get; set; }
    }
}