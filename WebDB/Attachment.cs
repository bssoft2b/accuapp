﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class Attachment
    {
        public Attachment()
        {
            AddOns = new HashSet<AddOn>();
            EmrRequests = new HashSet<EmrRequest>();
            TicketAttachments = new HashSet<TicketAttachment>();
        }

        [Key]
        public int AttachmentID { get; set; }
        [Required]
        [StringLength(250)]
        public string AttachmentName { get; set; }
        [Required]
        public string AttachmentContent { get; set; }

        [InverseProperty(nameof(AddOn.Attachment))]
        public virtual ICollection<AddOn> AddOns { get; set; }
        [InverseProperty(nameof(EmrRequest.Attachment))]
        public virtual ICollection<EmrRequest> EmrRequests { get; set; }
        [InverseProperty(nameof(TicketAttachment.Attachment))]
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
    }
}