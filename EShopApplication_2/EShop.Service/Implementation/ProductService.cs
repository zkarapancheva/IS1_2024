using EShop.Domain.Domain;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService (IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public void CreateNewProduct(Product p)
        {
            _productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = _productRepository.Get(id);
            _productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return _productRepository.Get(id);
        }

        public void UpdeteExistingProduct(Product p)
        {
            _productRepository.Update(p);
        }
    }
}
