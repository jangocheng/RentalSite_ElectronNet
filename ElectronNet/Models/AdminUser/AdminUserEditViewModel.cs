using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ElectronNet.Models
{
    public class AdminUserEditViewModel
    {
        public AdminUserFormModel AdminUser { get; set; }

        public List<Role> Roles { get; set; }

        public SelectList Cities { get; set; }

        public List<Role> SelRoles { get; set; }
    }
}
