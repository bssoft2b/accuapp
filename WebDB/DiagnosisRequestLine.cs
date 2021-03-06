// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Keyless]
    [Index(nameof(DiagnosisRequestID), Name = "IX_DiagnosisRequestLines_DiagnosisRequestID")]
    public partial class DiagnosisRequestLine
    {
        public int DiagnosisRequestLineID { get; set; }
        public int DiagnosisRequestID { get; set; }
        [Required]
        [StringLength(50)]
        public string UncoveredCode { get; set; }
        [Required]
        [StringLength(50)]
        public string CommonlyUsedDiagnoses { get; set; }
    }
}