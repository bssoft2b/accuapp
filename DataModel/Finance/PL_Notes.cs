﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Notes
    {
        [Column("Sales Rep")]
        [StringLength(50)]
        public string Sales_Rep { get; set; }
        public int? Group { get; set; }
        [Column(TypeName = "decimal(10, 3)")]
        public decimal? SubGroup { get; set; }
        public int? Account { get; set; }
        [Column("Account Name")]
        [StringLength(50)]
        public string Account_Name { get; set; }
        [StringLength(255)]
        public string Note { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        public bool? Status { get; set; }
    }
}