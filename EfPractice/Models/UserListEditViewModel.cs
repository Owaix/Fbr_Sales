using EfPractice.Areas.Identity.Data;
using System.Collections.Generic;

namespace EfPractice.Models
{
    public class UserListEditViewModel
    {
        public List<ApplicationUser> Users { get; set; } = new();
        public UserEditViewModel? User { get; set; }
    }
}
