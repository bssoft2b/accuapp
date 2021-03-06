// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class PatientDemographic
    {
        public PatientDemographic()
        {
            DiagnosisRequests_20Feb2020s = new HashSet<DiagnosisRequests_20Feb2020>();
        }

        [Key]
        [StringLength(50)]
        public string AccessionNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string ChartNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string SocialSecurity { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(10)]
        public string MiddleInitial { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [StringLength(3)]
        public string State { get; set; }
        [Required]
        [StringLength(20)]
        public string Zip { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [InverseProperty(nameof(DiagnosisRequests_20Feb2020.AccessionNumberNavigation))]
        public virtual ICollection<DiagnosisRequests_20Feb2020> DiagnosisRequests_20Feb2020s { get; set; }
    }
}