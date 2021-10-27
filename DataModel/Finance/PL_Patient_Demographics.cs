﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Patient_Demographics
    {
        [StringLength(50)]
        public string Accession { get; set; }
        [Column("Chart#")]
        [StringLength(50)]
        public string Chart_ { get; set; }
        [Column("Social Security")]
        [StringLength(50)]
        public string Social_Security { get; set; }
        [Column("Last Name")]
        [StringLength(50)]
        public string Last_Name { get; set; }
        [Column("First Name")]
        [StringLength(50)]
        public string First_Name { get; set; }
        [Column("Middle Initial")]
        [StringLength(10)]
        public string Middle_Initial { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(3)]
        public string State { get; set; }
        [StringLength(20)]
        public string Zip { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
    }
}