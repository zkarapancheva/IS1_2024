namespace Eshop.Web.Models.DTO
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCart>? Products { get; set;}
        public double TotalPrice { get; set;}
    }
}
