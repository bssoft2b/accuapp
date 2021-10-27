using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp32MVC.Models
{
    public class UserClaim
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string Value { get; set; }
    }
}
