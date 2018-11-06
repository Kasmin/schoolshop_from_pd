using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly int id = 1; // Before single user mode
        public ShopContext _db;

        public CartController(ShopContext db)
        {
            _db = db;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var cart = await _db.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.Id == id);
            List<CartItem> cartItems = cart.CartItems;

            return View(cartItems);
        }
    }
}
