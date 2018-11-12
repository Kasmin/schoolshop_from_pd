using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("orders")]
    public class OrdersController : Controller
    {
        public ShopContext _db;

        public OrdersController(ShopContext db)
        {
            _db = db;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.Number)
                .ToListAsync();

            var ordersViewModel = orders
                .Select(order => new OrderViewModel
                {
                    Number = order.Number,
                    Items = order.Items
                        .Select(item => new OrderItemViewModel
                        {
                            ProductName = item.Product.Name,
                            ProductPrice = item.Product.Price,
                            Count = item.Count
                        }).ToList()
                });

            return View(ordersViewModel);
        }
        [Route("make/{cartId:int}")]
        public async Task<IActionResult> Make(int cartId)
        {
            Cart cart = _db.Carts
                           .Include(c => c.CartItems)
                           .ThenInclude(ci => ci.Product)
                           .FirstOrDefault(c => c.Id == cartId);
            if (cart != null)
            {
                Order order = new Order();
                foreach (CartItem ci in cart.CartItems)
                {
                    order.Items.Add(new OrderItem(ci.Product, ci.CountOfProduct));

                }
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                // Temporary code block

                cart.CartItems.Clear();
                _db.Carts.Update(cart);
                await _db.SaveChangesAsync();
                // End of Temporary code block

                return RedirectToAction("Index");
            }

            throw new Exception("Корзина не существует");
        }
    }
}
