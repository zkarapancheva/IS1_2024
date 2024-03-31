using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class ProductInOrder : BaseEntity
    {

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
