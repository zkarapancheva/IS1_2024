namespace EShopAdminApplication_2.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string userId { get; set; }
        public EShopApplicationUser User { get; set; }
        public ICollection<ProductInOrder> productInOrders { get; set; }

    }
}
