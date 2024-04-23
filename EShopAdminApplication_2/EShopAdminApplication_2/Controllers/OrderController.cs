using EShopAdminApplication_2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace EShopAdminApplication_2.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5180/api/Admin/GetAllOrders";
            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        public IActionResult Details(Guid Id)
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5180/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = Id
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<Order>().Result;
            return View(data);
        }
    }
}
