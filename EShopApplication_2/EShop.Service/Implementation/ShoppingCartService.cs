using Eshop.DomainEntities;
using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;


        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Product> productRepository, IRepository<Order> orderRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository, IRepository<ProductInOrder> productInOrderRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _productInOrderRepository = productInOrderRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public bool AddToShoppingConfirmed(ProductInShoppingCart model, string userId)
        {
            if (userId == null)
                return false;


            var loggedInUser = _userRepository.Get(userId);
            var shoppingCart = loggedInUser?.ShoppingCart;

            if (shoppingCart.ProductInShoppingCarts == null)
                shoppingCart.ProductInShoppingCarts = new List<ProductInShoppingCart>(); ;

            shoppingCart.ProductInShoppingCarts.Add(model);
            _shoppingCartRepository.Update(shoppingCart);
            return true;
        }

        public bool deleteProductFromShoppingCart(string userId, Guid productId)
        {
            if (userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = loggedInUser?.ShoppingCart;
            var product = userShoppingCart?.ProductInShoppingCarts.Where(x => x.ProductId == productId).FirstOrDefault();

            userShoppingCart.ProductInShoppingCarts.Remove(product);
            _shoppingCartRepository.Update(userShoppingCart);
            return true;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = loggedInUser?.ShoppingCart;
            var allProduct = userShoppingCart?.ProductInShoppingCarts?.ToList();

            var totalPrice = allProduct.Select(x => (x.Product.Price * x.Quantity)).Sum();

            ShoppingCartDto dto = new ShoppingCartDto
            {
                Products = allProduct,
                TotalPrice = totalPrice
            };
            return dto;
        }

        public bool order(string userId)
        {
            if (userId == null)
                return false;

            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser?.ShoppingCart;
            EmailMessage message = new EmailMessage();
            message.Subject = "Successfull order";
            message.MailTo = loggedInUser.Email;
            Order order = new Order
            {
                Id = Guid.NewGuid(),
                userId = userId,
                User = loggedInUser
            };

            List<ProductInOrder> productsInOrder = new List<ProductInOrder>();

            var rez = userShoppingCart.ProductInShoppingCarts.Select(
                z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    Product = z.Product,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();


            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order is completed. The order conatins: ");

            for (int i = 1; i <= productsInOrder.Count(); i++)
            {
                var currentItem = productsInOrder[i - 1];
                totalPrice += currentItem.Quantity * currentItem.Product.Price;
                sb.AppendLine(i.ToString() + ". " + currentItem.Product.ProductName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Product.Price);
            }

            sb.AppendLine("Total price for your order: " + totalPrice.ToString());
            message.Content = sb.ToString();

            productsInOrder.AddRange(rez);

            foreach (var product in productsInOrder)
            {
                _productInOrderRepository.Insert(product);
            }

            loggedInUser.ShoppingCart.ProductInShoppingCarts.Clear();
            _userRepository.Update(loggedInUser);
            this._emailService.SendEmailAsync(message);

            return true;
        }
    }
}




