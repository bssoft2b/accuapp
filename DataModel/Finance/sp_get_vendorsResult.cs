// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Finance
{
    public partial class sp_get_vendorsResult
    {
        public int ID { get; set; }
        [Column("Vendor Name")]
        public string VendorName { get; set; }
        [Column("Installation Cost")]
        public decimal? InstallationCost { get; set; }
        [Column("Maintenance Cost")]
        public decimal? MaintenanceCost { get; set; }
        [Column("Comtron Installation")]
        public decimal? ComtronInstallation { get; set; }
        [Column("Annual Cost")]
        public decimal? AnnualCost { get; set; }
    }
}
