﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class t_RecentVisitsJan
    {
        [Required]
        [StringLength(50)]
        public string EMPLOYEE { get; set; }
        [Column(TypeName = "date")]
        public DateTime TIME { get; set; }
        [Required]
        [StringLength(50)]
        public string ROUTE_NAME { get; set; }
        [Required]
        [StringLength(50)]
        public string CUSTOMER_NAME { get; set; }
        [Required]
        [StringLength(50)]
        public string CHECKPOINT { get; set; }
        [StringLength(50)]
        public string CUSTOMER { get; set; }
    }
}