using Eshop.DomainEntities;
using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IEmailService _emailService;


        public ShoppingCartService (IRepository<ProductInOrder> _productInOrderRepository, IRepository<Order> _orderRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository, IEmailService emailService)
        {
            this._productInOrderRepository = _productInOrderRepository;
            this._orderRepository = _orderRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _emailService = emailService;
        }
        public bool AddToShoppingConfirmed(ProductInShoppingCart model, string userId)
        {

            var loggedInUser = _userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            if (userShoppingCart.ProductInShoppingCarts == null)
                userShoppingCart.ProductInShoppingCarts = new List<ProductInShoppingCart>(); ;

            userShoppingCart.ProductInShoppingCarts.Add(model);
            _shoppingCartRepository.Update(userShoppingCart);
            return true;
        }

        public bool deleteProductFromShoppingCart(string userId, Guid productId)
        {
            if (productId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                var product = userShoppingCart.ProductInShoppingCarts.Where(x => x.ProductId == productId).FirstOrDefault();

                userShoppingCart.ProductInShoppingCarts.Remove(product);

                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;

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
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.ShoppingCart;
                EmailMessage message = new EmailMessage();
                message.Subject = "Successfull order";
                message.MailTo = loggedInUser.Email;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    userId = userId,
                    Owner = loggedInUser
                };

                _orderRepository.Insert(order);

                List<ProductInOrder> productInOrder = new List<ProductInOrder>();

                var lista = userShoppingCart.ProductInShoppingCarts.Select(
                    x => new ProductInOrder
                    {
                        Id = Guid.NewGuid(),
                        ProductId = x.Product.Id,
                        Product = x.Product,
                        OrderId = order.Id,
                        Order = order,
                        Quantity = x.Quantity
                    }
                    ).ToList();


                StringBuilder sb = new StringBuilder();

                var totalPrice = 0.0;

                sb.AppendLine("Your order is completed. The order conatins: ");

                for (int i = 1; i <= lista.Count(); i++)
                {
                    var currentItem = lista[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.Product.Price;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Product.ProductName + " with quantity of: " + currentItem.Quantity + " and price of: $" + currentItem.Product.Price);
                }

                sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                message.Content = sb.ToString();

                productInOrder.AddRange(lista);

                foreach (var product in productInOrder)
                {
                    _productInOrderRepository.Insert(product);
                }

                loggedInUser.ShoppingCart.ProductInShoppingCarts.Clear();
                _userRepository.Update(loggedInUser);
                this._emailService.SendEmailAsync(message);

                return true;
            }
            return false;
        }
        
    }
}

