﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Keyless]
    [Table("DiagnosisRequests_Bkup20Apr")]
    public partial class DiagnosisRequests_Bkup20Apr
    {
        public int DiagnosisRequestID { get; set; }
        [Required]
        [StringLength(50)]
        public string AccessionNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RequestDate { get; set; }
        public int AccountID { get; set; }
        public int PhysicianID { get; set; }
        public string Signature { get; set; }
        [StringLength(50)]
        public string SignedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateSigned { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCompleted { get; set; }
        [StringLength(50)]
        public string CompletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastPrinted { get; set; }
        [StringLength(50)]
        public string LastPrintedBy { get; set; }
    }
}