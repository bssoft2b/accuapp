﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class PL_Accounts
    {
        [Key]
        public int Number { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int Group { get; set; }
        public int? SubGroup { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Suite { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [Column("Zip-Code")]
        [StringLength(9)]
        public string Zip_Code { get; set; }
        [StringLength(15)]
        public string Telephone { get; set; }
        [StringLength(15)]
        public string Fax { get; set; }
        [Column("SubGroup Grp")]
        public int? SubGroup_Grp { get; set; }
        public bool? Active { get; set; }
        [StringLength(20)]
        public string NPI { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string site { get; set; }
        [StringLength(50)]
        public string specialty { get; set; }
        [Column("multi-doctor")]
        public bool? multi_doctor { get; set; }
        [Column("business name")]
        [StringLength(255)]
        public string business_name { get; set; }
        [Column("Termination Date", TypeName = "date")]
        public DateTime? Termination_Date { get; set; }
        [Column("Inactive Date", TypeName = "date")]
        public DateTime? Inactive_Date { get; set; }
    }
}