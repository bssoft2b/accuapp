﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Client_Bill_Data
    {
        public int? Client { get; set; }
        [Column("Service Month", TypeName = "date")]
        public DateTime? Service_Month { get; set; }
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
        [Column("Bill Date", TypeName = "datetime")]
        public DateTime? Bill_Date { get; set; }
    }
}