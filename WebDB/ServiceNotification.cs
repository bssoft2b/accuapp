﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Table("ServiceNotification")]
    public partial class ServiceNotification
    {
        [Key]
        public long ServiceNotificationId { get; set; }
        [Required]
        [StringLength(450)]
        public string Email { get; set; }
        public long NotificationId { get; set; }
    }
}