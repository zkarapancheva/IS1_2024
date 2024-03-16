using Microsoft.AspNetCore.Identity;

namespace EShop.Web.Models
{
    public class EShopApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}
