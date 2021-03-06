// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    public partial class RoleReference
    {
        [Key]
        public int RoleReferenceID { get; set; }
        [Required]
        [StringLength(450)]
        public string UserID { get; set; }
        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }

        [ForeignKey(nameof(UserID))]
        [InverseProperty(nameof(AspNetUser.RoleReferences))]
        public virtual AspNetUser User { get; set; }
    }
}