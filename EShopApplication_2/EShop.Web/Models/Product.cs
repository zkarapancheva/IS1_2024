using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Models
{
    public class Product
    {
        [Key]
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
