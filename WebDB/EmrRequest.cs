﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class EmrRequest
    {
        public EmrRequest()
        {
            EmrNotes = new HashSet<EmrNote>();
            InvoiceRequestDetails = new HashSet<InvoiceRequestDetail>();
        }

        [Key]
        public int EMRRequestID { get; set; }
        public int AccountID { get; set; }
        [Required]
        [StringLength(50)]
        public string OfficeManagerName { get; set; }
        [StringLength(50)]
        public string OfficeManagerEmail { get; set; }
        [StringLength(50)]
        public string OfficeManagerPhone { get; set; }
        [StringLength(50)]
        public string PhysicianName { get; set; }
        [Required]
        [StringLength(50)]
        public string PhysicianSpecialty { get; set; }
        public int ExpectedSpecimenCount { get; set; }
        [StringLength(300)]
        public string CustomPanels { get; set; }
        public int VendorID { get; set; }
        [Required]
        [StringLength(50)]
        public string VendorContact { get; set; }
        [Required]
        [StringLength(50)]
        public string VendorPhone { get; set; }
        [StringLength(50)]
        public string VendorFax { get; set; }
        [Required]
        [StringLength(50)]
        public string VendorEmail { get; set; }
        [Required]
        [StringLength(50)]
        public string ConnectionType { get; set; }
        [Required]
        [StringLength(256)]
        public string RequestedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RequestedDate { get; set; }
        public double? VendorInstallationCost { get; set; }
        public double? VendorMaintenanceCost { get; set; }
        public double? VendorAnnualCost { get; set; }
        public double? ComtronInstallationCost { get; set; }
        public int RequestStatus { get; set; }
        [StringLength(250)]
        public string DenialReason { get; set; }
        public bool POSent { get; set; }
        public bool LISRequestSent { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProjectedLiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActualLiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ComtronTerminationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? VendorTerminationDate { get; set; }
        public bool HasCustomPanels { get; set; }
        public int? AttachmentID { get; set; }
        [StringLength(256)]
        public string QuoteApprovedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? QuoteApprovedDate { get; set; }

        [ForeignKey(nameof(AccountID))]
        [InverseProperty("EmrRequests")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(AttachmentID))]
        [InverseProperty("EmrRequests")]
        public virtual Attachment Attachment { get; set; }
        [InverseProperty(nameof(EmrNote.EmrRequest))]
        public virtual ICollection<EmrNote> EmrNotes { get; set; }
        [InverseProperty(nameof(InvoiceRequestDetail.EmrRequest))]
        public virtual ICollection<InvoiceRequestDetail> InvoiceRequestDetails { get; set; }
    }

    public class EMRRequestViewModel
    {
        public EmrRequest emrRequest { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public ICollection<EmrNote> emrNote { get; set; }
        //public string DeliveryMethod { get; set; }
        public string DenialReason { get; set; }

        public List<String> Statuses = new List<String>() { "In process", "Testing", "Denied", "Canceled", "Live", "Disconnected", "On Hold", "New", "Pending Quote Approval", "Quote Approved", "Quote Requested", "Acknowledged" };

    }
}