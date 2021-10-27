﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class HardwareRequest
    {
        public HardwareRequest()
        {
            HardwareItems = new HashSet<HardwareItem>();
            HardwareRequestAccounts = new HashSet<HardwareRequestAccount>();
            HardwareRequestNotes = new HashSet<HardwareRequestNote>();
        }

        [Key]
        public int HardwareRequestID { get; set; }
        [Required]
        [StringLength(255)]
        public string AssignedTo { get; set; }
        public int HardwareRequestStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateRequested { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateModified { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateShipped { get; set; }
        [StringLength(50)]
        public string TrackingNumber { get; set; }
        public int? DeliveryMethodID { get; set; }

        [InverseProperty(nameof(HardwareItem.HardwareRequest))]
        public virtual ICollection<HardwareItem> HardwareItems { get; set; }
        [InverseProperty(nameof(HardwareRequestAccount.HardwareRequest))]
        public virtual ICollection<HardwareRequestAccount> HardwareRequestAccounts { get; set; }
        [InverseProperty(nameof(HardwareRequestNote.HardwareRequest))]
        public virtual ICollection<HardwareRequestNote> HardwareRequestNotes { get; set; }
    }
}