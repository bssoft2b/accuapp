﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Merabi_Imports
    {
        [Column(TypeName = "date")]
        public DateTime? date { get; set; }
        [Column(TypeName = "money")]
        public decimal? amount { get; set; }
    }
}