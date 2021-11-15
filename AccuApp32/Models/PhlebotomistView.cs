using PhlebotomyDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp32MVC.Models
{
    public class PhlebotomistView
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public string Zip { get; set; }
        public int[] Leads { get; set; }
        public DateTime? EmployeeStartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public List<PhlebotomistAssignmentLine> PhlebotomistAssignmentLines { get; set; }
    }
}
