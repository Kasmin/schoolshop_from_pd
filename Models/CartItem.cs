using System;
namespace Shop.Models
{
    public class CartItem
    {
        private CartItem()
        { }

        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int CountOfProduct { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }

        //public CartItem()
        //{
        //}
    }
}
