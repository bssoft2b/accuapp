// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class UOM
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [Column("UOM")]
        [StringLength(50)]
        public string UOM1 { get; set; }
    }
}