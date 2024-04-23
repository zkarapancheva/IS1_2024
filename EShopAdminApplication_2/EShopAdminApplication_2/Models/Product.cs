using System.ComponentModel.DataAnnotations;

namespace EShopAdminApplication_2.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductDescription { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
