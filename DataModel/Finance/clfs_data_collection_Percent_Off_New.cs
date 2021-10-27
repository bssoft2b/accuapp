﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    [Table("clfs_data_collection_Percent Off_New")]
    public partial class clfs_data_collection_Percent_Off_New
    {
        [Column("HCPCS CODE(5-alpha numeric characters)")]
        [StringLength(50)]
        public string HCPCS_CODE_5_alpha_numeric_characters_ { get; set; }
        [Column("PAYMENT RATE(1-5 numeric characters and two decimal places)")]
        [StringLength(50)]
        public string PAYMENT_RATE_1_5_numeric_characters_and_two_decimal_places_ { get; set; }
        [Column("VOLUME(1-6 numeric characters)")]
        [StringLength(50)]
        public string VOLUME_1_6_numeric_characters_ { get; set; }
        [Column("2019 Payment w  Cap")]
        [StringLength(50)]
        public string _2019_Payment_w__Cap { get; set; }
        [Column("2020 Payment w  Cap")]
        [StringLength(50)]
        public string _2020_Payment_w__Cap { get; set; }
        [Column("Year When Full Payment Change Applies")]
        [StringLength(50)]
        public string Year_When_Full_Payment_Change_Applies { get; set; }
        [Column("Percent Off")]
        [StringLength(50)]
        public string Percent_Off { get; set; }
    }
}