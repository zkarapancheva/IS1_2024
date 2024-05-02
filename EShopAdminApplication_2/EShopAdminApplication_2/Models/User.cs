using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EShopAdminApplication_2.Models
{
    public class User
    {
        public String Email { get; set; }
        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
    }
}
