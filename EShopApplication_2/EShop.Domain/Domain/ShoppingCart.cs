using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public EShopApplicationUser? Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart>? ProductInShoppingCarts { get; set; }

    }
}
