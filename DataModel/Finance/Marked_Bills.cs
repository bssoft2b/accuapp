﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Marked_Bills
    {
        [Key]
        [StringLength(50)]
        public string User { get; set; }
        [Key]
        [Column("Inv#")]
        [StringLength(50)]
        public string Inv_ { get; set; }
        [Key]
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
        public int? Active { get; set; }
        [Column("Bill-Type")]
        public int? Bill_Type { get; set; }
        [StringLength(255)]
        public string Reason { get; set; }
    }
}