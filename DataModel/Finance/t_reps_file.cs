﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class t_reps_file
    {
        public int? Department { get; set; }
        [StringLength(50)]
        public string FName { get; set; }
        [StringLength(50)]
        public string LName { get; set; }
        [StringLength(10)]
        public string EmployeeID { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [Column(TypeName = "money")]
        public decimal? Rate { get; set; }
        [StringLength(10)]
        public string Type { get; set; }
        [StringLength(15)]
        public string SSN { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Hired { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Rehired { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Fired { get; set; }
        [StringLength(50)]
        public string HomePhone { get; set; }
        [StringLength(50)]
        public string PersonalMobile { get; set; }
        [StringLength(250)]
        public string WorkEmail { get; set; }
        [StringLength(50)]
        public string WorkMobile { get; set; }
        [StringLength(50)]
        public string WorkPhone { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }
    }
}