// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Rent_Data
    {
        public int? Account { get; set; }
        [StringLength(255)]
        public string Doctor { get; set; }
        [Column("written to")]
        [StringLength(500)]
        public string written_to { get; set; }
        [StringLength(500)]
        public string location { get; set; }
        [StringLength(20)]
        public string telephone { get; set; }
        [Column(TypeName = "money")]
        public decimal? total { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column(TypeName = "money")]
        public decimal? Current { get; set; }
        [Column("security deposit", TypeName = "money")]
        public decimal? security_deposit { get; set; }
        [Column("begin date", TypeName = "date")]
        public DateTime? begin_date { get; set; }
        [StringLength(500)]
        public string remark { get; set; }
        [Key]
        public int key { get; set; }
        public bool? Terminated { get; set; }
        public bool Approved { get; set; }
    }
}