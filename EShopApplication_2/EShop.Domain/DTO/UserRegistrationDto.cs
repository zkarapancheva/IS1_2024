using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.DTO
{
    public class UserRegistrationDto
    {
        public String Email { get; set; }
        public String Password { get; set; }    
        public String ConfirmPassword { get; set; }
    }
}
