﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Lab_Express_Staging
    {
        public int? Account { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public string Amount { get; set; }
    }
}