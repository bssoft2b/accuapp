﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDB
{
    public partial class sp_get_coding_invoicesResult
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string InvoiceNo { get; set; }
        public int BillType { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int Key { get; set; }
        public bool? Active { get; set; }
        public string PrevBillType { get; set; }
    }
}
