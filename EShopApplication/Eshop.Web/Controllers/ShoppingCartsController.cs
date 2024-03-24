//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Eshop.Web.Data;
//using Eshop.Web.Models;
//using System.Security.Claims;
//using Eshop.Web.Models.DTO;

//namespace Eshop.Web.Controllers
//{
//    public class ShoppingCartsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ShoppingCartsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: ShoppingCarts
//        public async Task<IActionResult> Index()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var loggedInUser = _context.Users
//                .Include(z => z.ShoppingCart)
//                .Include("ShoppingCart.ProductInShoppingCarts")
//                .Include("ShoppingCart.ProductInShoppingCarts.Product")
//                .FirstOrDefault(x => x.Id == userId);

//            var userShoppingCart = loggedInUser?.ShoppingCart;
//            var allProduct = userShoppingCart?.ProductInShoppingCarts?.ToList();

//            var totalPrice = allProduct.Select(x => (x.Product.Price * x.Quantity)).Sum();

//            ShoppingCartDto dto = new ShoppingCartDto
//            {
//                Products = allProduct,
//                TotalPrice = totalPrice
//            };

//            return View(dto);
//            //return (IActionResult)dto;
//        }


//        public IActionResult DeleteFromShoppingCart(Guid id)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            var loggedInUser = _context.Users
//                .Include(z => z.ShoppingCart)
//                .Include("ShoppingCart.ProductInShoppingCarts")
//                .Include("ShoppingCart.ProductInShoppingCarts.Product")
//                .FirstOrDefault(x => x.Id == userId);

//            var userShoppingCart = loggedInUser.ShoppingCart;
//            var product = userShoppingCart.ProductInShoppingCarts.Where(x => x.ProductId == id).FirstOrDefault();

//            userShoppingCart.ProductInShoppingCarts.Remove(product);
//            _context.ShoppingCarts.Update(userShoppingCart);
//            _context.SaveChanges();
//            return RedirectToAction("Index");

//        }

//        // GET: ShoppingCarts/Details/5
//        public async Task<IActionResult> Details(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var shoppingCart = await _context.ShoppingCarts
//                .Include(s => s.Owner)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (shoppingCart == null)
//            {
//                return NotFound();
//            }

//            return View(shoppingCart);
//        }

//        // GET: ShoppingCarts/Create
//        public IActionResult Create()
//        {
//            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
//            return View();
//        }

//        // POST: ShoppingCarts/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,OwnerId")] ShoppingCart shoppingCart)
//        {
//            if (ModelState.IsValid)
//            {
//                shoppingCart.Id = Guid.NewGuid();
//                _context.Add(shoppingCart);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
//            return View(shoppingCart);
//        }

//        // GET: ShoppingCarts/Edit/5
//        public async Task<IActionResult> Edit(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
//            if (shoppingCart == null)
//            {
//                return NotFound();
//            }
//            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
//            return View(shoppingCart);
//        }

//        // POST: ShoppingCarts/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(Guid id, [Bind("Id,OwnerId")] ShoppingCart shoppingCart)
//        {
//            if (id != shoppingCart.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(shoppingCart);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ShoppingCartExists(shoppingCart.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
//            return View(shoppingCart);
//        }

//        // GET: ShoppingCarts/Delete/5
//        public async Task<IActionResult> Delete(Guid? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var shoppingCart = await _context.ShoppingCarts
//                .Include(s => s.Owner)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (shoppingCart == null)
//            {
//                return NotFound();
//            }

//            return View(shoppingCart);
//        }

//        // POST: ShoppingCarts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(Guid id)
//        {
//            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
//            if (shoppingCart != null)
//            {
//                _context.ShoppingCarts.Remove(shoppingCart);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ShoppingCartExists(Guid id)
//        {
//            return _context.ShoppingCarts.Any(e => e.Id == id);
//        }

//        public IActionResult order ()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            //if(userId == null)
//            var loggedInUser = _context.Users
//                .Include(z => z.ShoppingCart)
//                .Include("ShoppingCart.ProductInShoppingCarts")
//                .Include("ShoppingCart.ProductInShoppingCarts.Product")
//                .FirstOrDefault(x => x.Id == userId);

//            var userShoppingCart = loggedInUser.ShoppingCart;

//            Order order = new Order
//            {
//                Id = Guid.NewGuid(),
//                userId = userId,
//                Owner = loggedInUser
//            };

//            _context.Orders.Add(order);
//            _context.SaveChanges();

//            List<ProductInOrder> productInOrder = new List<ProductInOrder>(); 

//            var lista = userShoppingCart.ProductInShoppingCarts.Select(
//                x => new ProductInOrder
//                {
//                    Id = Guid.NewGuid(),
//                    ProductId = x.Product.Id,
//                    Product = x.Product,
//                    OrderId = order.Id,
//                    Order = order,
//                    Quantity = x.Quantity
//                }
//                ).ToList();

//            productInOrder.AddRange( lista );

//            foreach( var product in productInOrder)
//            {
//                _context.ProductInOrders.Add( product );
//            }

//            loggedInUser.ShoppingCart.ProductInShoppingCarts.Clear();
//            _context.Users.Update(loggedInUser);
//            _context.SaveChanges();

//            return RedirectToAction("Index", "ShoppingCart");


//        }
//    }
//}
