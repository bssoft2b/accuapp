// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class priceimport
    {
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column("M Care Price")]
        public float? M_Care_Price { get; set; }
        [Column("M Caid Price")]
        public float? M_Caid_Price { get; set; }
        public float? Cost { get; set; }
        [Column("Sch 0 ")]
        public float? Sch_0_ { get; set; }
    }
}