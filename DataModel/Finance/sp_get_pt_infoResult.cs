// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class sp_get_pt_infoResult
    {
        public string Col0 { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? Col2 { get; set; }
        [Column("Client#   ")]
        public string Client { get; set; }
        public string Name { get; set; }
        public string Fax { get; set; }
    }
}
