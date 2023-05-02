using Microsoft.AspNetCore.Identity;

namespace Ecommerce_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
