﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class tbl_Landmark_Dec
    {
        [Column("Inv#")]
        [StringLength(50)]
        public string Inv_ { get; set; }
        [StringLength(50)]
        public string PatientsName { get; set; }
        [StringLength(50)]
        public string BillType { get; set; }
        [StringLength(50)]
        public string SrvDate { get; set; }
        [StringLength(50)]
        public string TrnDate { get; set; }
        [Column("Column 5")]
        [StringLength(50)]
        public string Column_5 { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(50)]
        public string CPT { get; set; }
        [StringLength(50)]
        public string TestName { get; set; }
        [StringLength(50)]
        public string Units { get; set; }
        [Column(TypeName = "money")]
        public decimal? Charges { get; set; }
        [Column(TypeName = "money")]
        public decimal? Payments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Adjustments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
        [StringLength(50)]
        public string SalesGroup1 { get; set; }
        [Column("Client#")]
        [StringLength(50)]
        public string Client_ { get; set; }
        [StringLength(50)]
        public string ClientName { get; set; }
        [Column("Physician#")]
        [StringLength(50)]
        public string Physician_ { get; set; }
    }
}