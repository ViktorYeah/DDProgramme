using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DoubleMastersDegreeProgramme.ViewModels
{
    public class ChangeRoleViewModel
    {
        // This model will allow to manage 
        // all the roles for one user (one 
        // user can have multiple roles).

        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
