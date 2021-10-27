﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class t_1435
    {
        [Column(TypeName = "date")]
        public DateTime? CollectDate { get; set; }
        [Column("Client#")]
        public int? Client_ { get; set; }
        public string ClientName { get; set; }
        [Column("Chart#")]
        public string Chart_ { get; set; }
        [Column("Inv#")]
        public string Inv_ { get; set; }
        public string Name { get; set; }
        public int? Billtype { get; set; }
        public string Insurance { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Srvdate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Trndate { get; set; }
        public string type { get; set; }
        public string Doctor { get; set; }
        [Column("Last Billed", TypeName = "date")]
        public DateTime? Last_Billed { get; set; }
        [Column(TypeName = "money")]
        public decimal? Charges { get; set; }
        [Column(TypeName = "money")]
        public decimal? Payments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Adjusts { get; set; }
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
    }
}