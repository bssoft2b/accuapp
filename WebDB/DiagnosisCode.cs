﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class DiagnosisCode
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
    }
}