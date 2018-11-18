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
            if (cartItems.Count == 0)
            {
                TempData["cartEmpty"] = true;
            } else {
                TempData["cartEmpty"] = false;
                TempData["cartId"] = cart.Id;
            }

            return View(cartItems);
        }

        [HttpGet]
        [Route("add/{productId:int}", Name = "AddProductToCart")]
        public async Task<IActionResult> Add(int productId, int count = 1, int cartId = 1)
        {
            Product productToCart = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            CartItem existCartItem = await _db.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId);

            if (productToCart != null)
            {
                CartItem _cartItem;
                if (existCartItem != null)
                {
                    existCartItem.CountOfProduct += count;
                    _db.CartItems.Update(existCartItem);
                    _cartItem = existCartItem;
                    TempData["productCount"] = count;
                }
                else
                {
                    _cartItem = new CartItem(productToCart, count, cartId);
                    _db.CartItems.Add(_cartItem);
                    TempData["productCount"] = _cartItem.CountOfProduct;
                }
                await _db.SaveChangesAsync();
                ProductToCartView jsonBody = new ProductToCartView()
                {
                    ProductName = _cartItem.Product.Name,
                    ProductPrice = _cartItem.Product.Price,
                    ProductCount = count
                };
                
                return Json(jsonBody);
            }

            throw new Exception("Неверный ID продукта");
        }

        [HttpGet]
        [Route("delete/{cartID:int}", Name = "DeleteProductFromCart")]
        public async Task<IActionResult> Delete(int cartId, int count)
        {
            CartItem cartItem = await _db.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartId);
            if (cartItem != null)
            {
                if (cartItem.CountOfProduct <= count)
                {
                    _db.CartItems.Remove(cartItem);
                }
                else if (count > 0)
                {
                    cartItem.CountOfProduct -= count;
                    _db.CartItems.Update(cartItem);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            throw new Exception("Ошибка удаления CartItem");
        }
    }
}
