// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Rep_Stop_Periods
    {
        public int? ID { get; set; }
        [Column("Stop Date", TypeName = "date")]
        public DateTime? Stop_Date { get; set; }
        [Column("Start Date", TypeName = "date")]
        public DateTime? Start_Date { get; set; }
    }
}