// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class dim_opportunity
    {
        [Key]
        public int opportunity_id { get; set; }
        [StringLength(20)]
        public string opportunity_name { get; set; }
        public int? account_Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? created_Date { get; set; }
        [StringLength(20)]
        public string account_Owner { get; set; }
    }
}