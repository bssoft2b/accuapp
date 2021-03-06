// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class TicketAttachment
    {
        [Key]
        public int TicketAttachmentID { get; set; }
        public int TicketID { get; set; }
        public int AttachmentID { get; set; }

        [ForeignKey(nameof(AttachmentID))]
        [InverseProperty("TicketAttachments")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(TicketID))]
        [InverseProperty("TicketAttachments")]
        public virtual Ticket Ticket { get; set; }
    }
}