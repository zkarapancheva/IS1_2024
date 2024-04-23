using EShop.Domain.Domain;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public AdminController (IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetDetails(BaseEntity id)
        {
            return this._orderService.GetDetailsForOrder(id);
        }

    }
}
