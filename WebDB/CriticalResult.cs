// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class CriticalResult
    {
        public CriticalResult()
        {
            CriticalResultLines = new HashSet<CriticalResultLine>();
        }

        [Key]
        public int CriticalResultID { get; set; }
        [Required]
        [StringLength(50)]
        public string AccessionNumber { get; set; }
        public int AccountID { get; set; }
        public bool Completed { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastWorkedDate { get; set; }

        [ForeignKey(nameof(AccountID))]
        [InverseProperty("CriticalResults")]
        public virtual Account Account { get; set; }
        [InverseProperty(nameof(CriticalResultLine.CriticalResult))]
        public virtual ICollection<CriticalResultLine> CriticalResultLines { get; set; }
    }
}