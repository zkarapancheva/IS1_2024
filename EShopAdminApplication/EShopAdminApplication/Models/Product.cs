using System.ComponentModel.DataAnnotations;

namespace EShopAdminApplication.Models
{
    public class Product
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
