using EShop.Domain.Domain;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using Microsoft.EntityFrameworkCore;
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
        public ShoppingCartService (IRepository<ProductInOrder> _productInOrderRepository, IRepository<Order> _orderRepository, IUserRepository userRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository)
        {
            this._productInOrderRepository = _productInOrderRepository;
            this._orderRepository = _orderRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
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

                productInOrder.AddRange(lista);

                foreach (var product in productInOrder)
                {
                    _productInOrderRepository.Insert(product);
                }

                loggedInUser.ShoppingCart.ProductInShoppingCarts.Clear();
                _userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }
        
    }
}
