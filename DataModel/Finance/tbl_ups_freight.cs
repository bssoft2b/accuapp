﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class tbl_ups_freight
    {
        public int? ID { get; set; }
        public string Track_Num { get; set; }
        public int? Account { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvDate { get; set; }
        [Column(TypeName = "money")]
        public decimal? Tot_net { get; set; }
    }
}