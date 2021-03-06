// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Finance_Change_Tracking
    {
        [StringLength(100)]
        public string Table { get; set; }
        [Column("Line Identifier")]
        [StringLength(100)]
        public string Line_Identifier { get; set; }
        [Column("date changed", TypeName = "datetime")]
        public DateTime? date_changed { get; set; }
        [StringLength(50)]
        public string User { get; set; }
        [StringLength(6)]
        public string action { get; set; }
    }
}