// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class sp_get_client_billingResult
    {
        public int? Client { get; set; }
        [Column("Service Month")]
        public DateTime? ServiceMonth { get; set; }
        public decimal? Balance { get; set; }
        [Column("Bill Date")]
        public DateTime? BillDate { get; set; }
        [Column("Bill Number")]
        public string BillNumber { get; set; }
        public decimal? Balance1 { get; set; }
        public int? Client1 { get; set; }
    }
}
