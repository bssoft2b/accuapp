﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class all_payments_by_patient_name
    {
        [Column("Inv #")]
        [StringLength(50)]
        public string Inv__ { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column("Bill-type")]
        [StringLength(50)]
        public string Bill_type { get; set; }
        [Column("Srv date")]
        [StringLength(50)]
        public string Srv_date { get; set; }
        [Column("Trn date")]
        [StringLength(50)]
        public string Trn_date { get; set; }
        [Column("Last Billed")]
        [StringLength(50)]
        public string Last_Billed { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(50)]
        public string CPT { get; set; }
        [StringLength(50)]
        public string TestName { get; set; }
        [StringLength(50)]
        public string Units { get; set; }
        [Column(TypeName = "money")]
        public decimal? Charges { get; set; }
        [Column(TypeName = "money")]
        public decimal? Payments { get; set; }
        [Column(TypeName = "money")]
        public decimal? Adjusts { get; set; }
        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }
        [Column("Sales Group")]
        [StringLength(50)]
        public string Sales_Group { get; set; }
        [Column("Client #")]
        [StringLength(50)]
        public string Client__ { get; set; }
        [Column("Client Name")]
        [StringLength(50)]
        public string Client_Name { get; set; }
    }
}