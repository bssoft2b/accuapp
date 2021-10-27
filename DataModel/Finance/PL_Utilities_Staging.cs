﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Utilities_Staging
    {
        public int? TrackID { get; set; }
        [Column("Utl.Type")]
        [StringLength(50)]
        public string Utl_Type { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column("Utl.Num")]
        [StringLength(50)]
        public string Utl_Num { get; set; }
        [Column("Utl.Name")]
        [StringLength(50)]
        public string Utl_Name { get; set; }
        [Column("Utl.Memo")]
        [StringLength(500)]
        public string Utl_Memo { get; set; }
        [StringLength(50)]
        public string Accu_Account { get; set; }
        [StringLength(50)]
        public string Clr { get; set; }
        [StringLength(50)]
        public string Split { get; set; }
        [StringLength(50)]
        public string Debit { get; set; }
        [StringLength(50)]
        public string Accu_Group { get; set; }
        [StringLength(100)]
        public string Notes { get; set; }
    }
}