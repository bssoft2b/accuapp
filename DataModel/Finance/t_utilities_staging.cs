﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class t_utilities_staging
    {
        [StringLength(50)]
        public string Row { get; set; }
        [StringLength(50)]
        public string pass1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [StringLength(50)]
        public string pass2 { get; set; }
        [StringLength(255)]
        public string Account { get; set; }
        [StringLength(50)]
        public string Pass3 { get; set; }
        [StringLength(255)]
        public string Company { get; set; }
        [StringLength(50)]
        public string pass4 { get; set; }
        public string Memo { get; set; }
        [StringLength(50)]
        public string pass5 { get; set; }
        [StringLength(250)]
        public string pass6 { get; set; }
        [StringLength(250)]
        public string pass7 { get; set; }
        [StringLength(250)]
        public string pass8 { get; set; }
        [StringLength(250)]
        public string pass9 { get; set; }
        [StringLength(250)]
        public string pass10 { get; set; }
        [StringLength(250)]
        public string pass11 { get; set; }
        [Column(TypeName = "money")]
        public decimal? debit { get; set; }
    }
}