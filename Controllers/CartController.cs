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
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly int id = 1; // Before single user mode
        public ShopContext _db;

        public CartController(ShopContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var cart = await _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
            List<CartItem> cartItems = cart.CartItems;
            ViewData["cartID"] = cart.Id;
            return View(cartItems);
        }

        [HttpGet]
        [Route("add/{productID:int}", Name = "AddProductToCart")]
        public async Task<IActionResult> Add(int productID, int count = 1, int cartID = 1)
        {
            Product productToCart = await _db.Products.FirstOrDefaultAsync(p => p.Id == productID);
            CartItem existCartItem = await _db.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productID);
            
            if(existCartItem != null)
            {
                existCartItem.CountOfProduct += count;
                _db.CartItems.Attach(existCartItem);
                _db.Entry(existCartItem).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                ViewData["productCount"] = count;
                return View(existCartItem);
            }
            else if (productToCart != null)
            {
                CartItem cartItem = new CartItem(productToCart, count, cartID);
                _db.CartItems.Add(cartItem);
                await _db.SaveChangesAsync();
                ViewData["productCount"] = cartItem.CountOfProduct;
                return View(cartItem);
            } 

            throw new Exception("Неверный ID продукта");
        }

        [HttpGet]
        [Route("delete/{cartID:int}", Name = "DeleteProductFromCart")]
        public async Task<IActionResult> Delete(int cartID, int count)
        {
            CartItem cartItem = await _db.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartID);
            if(cartItem != null)
            {
                if(cartItem.CountOfProduct <= count)
                {
                    _db.CartItems.Remove(cartItem);
                } else if(count > 0)
                {
                    cartItem.CountOfProduct -= count;
                    _db.CartItems.Attach(cartItem);
                    _db.Entry(cartItem).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            } 

            throw new Exception("Ошибка удаления CartItem");
        }
    }
}
