﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class Coding_Activity
    {
        [StringLength(50)]
        public string User { get; set; }
        [StringLength(50)]
        public string Table { get; set; }
        [StringLength(50)]
        public string PKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [StringLength(255)]
        public string Change { get; set; }
    }
}