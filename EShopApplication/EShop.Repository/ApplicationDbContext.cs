using Eshop.DomainEntities;
using EShop.Domain.Domain;
using EShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<EShopApplicationUser>
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
