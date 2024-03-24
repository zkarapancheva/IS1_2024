using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string userId { get; set; }
        public EShopApplicationUser User { get; set; }
        public ICollection<ProductInOrder> productInOrders { get; set; }

    }
}
