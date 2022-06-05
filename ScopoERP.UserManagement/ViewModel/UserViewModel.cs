using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoERP.UserManagement.ViewModel
{
    public class UserViewModel
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public List<string> RoleNames { get; set; }
        public string Email { get; set; }

        public int? AccountId { get; set; }
        public string AccountName { get; set; }


        public string RolesToString()
        {
            return string.Join(", ", RoleNames);
        }
    }
}
