// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class yearend
    {
        [Column("Collect-Date  ")]
        [StringLength(50)]
        public string Collect_Date__ { get; set; }
        [Column("Client#   ")]
        [StringLength(50)]
        public string Client____ { get; set; }
        [Column("Client-Name     ")]
        [StringLength(50)]
        public string Client_Name_____ { get; set; }
        [Column("Chart#    ")]
        [StringLength(50)]
        public string Chart_____ { get; set; }
        [Required]
        [Column("Inv #         ")]
        [StringLength(50)]
        public string Inv___________ { get; set; }
        [Column("Name                 ")]
        [StringLength(50)]
        public string Name_________________ { get; set; }
        [Column("Bill-type  ")]
        public int Bill_type__ { get; set; }
        [Column("Bill-type Insurance ")]
        [StringLength(255)]
        public string Bill_type_Insurance_ { get; set; }
        [Column("Srv date  ")]
        [StringLength(50)]
        public string Srv_date__ { get; set; }
        [Column("Trn date  ")]
        [StringLength(50)]
        public string Trn_date__ { get; set; }
        [Column("type    ")]
        [StringLength(50)]
        public string type____ { get; set; }
        [Column("Doctor     ")]
        [StringLength(50)]
        public string Doctor_____ { get; set; }
        [Column("Last Billed ")]
        [StringLength(50)]
        public string Last_Billed_ { get; set; }
        [Column("Charges  ", TypeName = "money")]
        public decimal? Charges__ { get; set; }
        [Column("Payments  ", TypeName = "money")]
        public decimal? Payments__ { get; set; }
        [Column("Adjusts   ", TypeName = "money")]
        public decimal? Adjusts___ { get; set; }
        [Column("Balance ", TypeName = "money")]
        public decimal? Balance_ { get; set; }
        [Column("Insurance-1 ")]
        [StringLength(50)]
        public string Insurance_1_ { get; set; }
        [Column("Ins1-Name ")]
        [StringLength(50)]
        public string Ins1_Name_ { get; set; }
        [Column("Insurance-2 ")]
        [StringLength(50)]
        public string Insurance_2_ { get; set; }
        [Column("Ins2-Name ")]
        [StringLength(50)]
        public string Ins2_Name_ { get; set; }
        [Column("Class ")]
        [StringLength(50)]
        public string Class_ { get; set; }
        [Column("Sales Group1")]
        [StringLength(50)]
        public string Sales_Group1 { get; set; }
        [Column("Physician #")]
        public int? Physician__ { get; set; }
    }
}