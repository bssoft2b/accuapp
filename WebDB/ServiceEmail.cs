// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Table("ServiceEmail")]
    public partial class ServiceEmail
    {
        [Key]
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
    }
}