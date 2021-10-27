using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;

namespace AccuApp32MVC.Models
{
    public class DiagnosisRequestsView
    {
        public DiagnosisRequest DiagnosisRequest { get; set; }
        public List<DiagnosisRequestLine> DiagnosisRequestLines { get; set; }
        public List<DiagnosisLine> DiagnosisLines { get; set; }

        public string ServiceDate { get; set; }
    }
}
