// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebDB
{
    [Index(nameof(NormalizedEmail), Name = "EmailIndex")]
    [Index(nameof(NormalizedUserName), Name = "UserNameIndex", IsUnique = true)]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            RoleReferences = new HashSet<RoleReference>();
        }

        [Key]
        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        public DateTime? LastLogin { get; set; }

        [InverseProperty("User")]
        public virtual RegistrationDetail RegistrationDetail { get; set; }
        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        [InverseProperty(nameof(RoleReference.User))]
        public virtual ICollection<RoleReference> RoleReferences { get; set; }
    }
}