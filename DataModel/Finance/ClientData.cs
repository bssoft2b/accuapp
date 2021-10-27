﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class ClientData
    {
        [Key]
        public long ClientId { get; set; }
        [Required]
        [StringLength(20)]
        public string Data { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedDateUtc { get; set; }
    }
}