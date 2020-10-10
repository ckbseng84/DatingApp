using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{
    public class UserRole: IdentityRole<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}