using System;
namespace Shop.Models
{
    public class CartItem
    {
        private CartItem()
        { }
        public CartItem(Product product, int count)
        {
            Product = product;
            CountOfProduct = count;
        }

        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int CountOfProduct { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }

    }
}
