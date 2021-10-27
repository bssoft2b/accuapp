﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class Vendor
    {
        public Vendor()
        {
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int VendorID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public double? VendorInstallationCost { get; set; }
        public double? VendorMaintenanceCost { get; set; }
        public double? VendorAnnualCost { get; set; }
        public double? ComtronInstallationCost { get; set; }

        [InverseProperty(nameof(Ticket.Vendor))]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}