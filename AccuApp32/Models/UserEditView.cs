using DataModel.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDB;

namespace AccuApp32MVC.Models
{
    public class UserEditView
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Role> Roles { get; set; }
        public List<UserClaim> UserClaims { get; set; }
        public List<string> ClaimTypes { get; set; }
    }
}
