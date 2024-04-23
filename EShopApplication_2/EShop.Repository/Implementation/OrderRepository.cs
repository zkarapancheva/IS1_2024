using EShop.Domain.Domain;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> _orders;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            _orders = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return _orders
                .Include(z => z.productInOrders)
                .Include(z => z.User)
                .Include("productInOrders.Product")
                .ToList();
        }

        public Order GetDetailsForOrder(BaseEntity model)
        {
            return _orders
                .Include(z => z.productInOrders)
                .Include(z => z.User)
                .Include("productInOrders.Product")
                .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}
