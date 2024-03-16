using System.ComponentModel.DataAnnotations;

namespace Eshop.Web.Models
{
    public class ProductInShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShoppingCartId { get; set; }
        public Product? Product { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
