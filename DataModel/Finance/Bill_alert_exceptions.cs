﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Bill_alert_exceptions
    {
        [Key]
        [Column("Bill-Type")]
        public int Bill_Type { get; set; }
    }
}