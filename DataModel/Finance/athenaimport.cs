﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class athenaimport
    {
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        [StringLength(50)]
        public string lname { get; set; }
        [StringLength(50)]
        public string fname { get; set; }
        [StringLength(255)]
        public string practname { get; set; }
        [StringLength(255)]
        public string doctor { get; set; }
        [StringLength(255)]
        public string groupname { get; set; }
        [StringLength(255)]
        public string recprac { get; set; }
        [StringLength(255)]
        public string recprov { get; set; }
        public string orderdesc { get; set; }
        public string orderid { get; set; }
        [StringLength(50)]
        public string ordertype { get; set; }
        [StringLength(50)]
        public string clinid { get; set; }
        [Column("client#")]
        public int? client_ { get; set; }
    }
}