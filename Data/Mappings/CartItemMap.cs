using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Models;

namespace Shop.Data.Mappings
{
    public class CartItemMap : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(ci => ci.Cart).WithMany(c => c.CartItems).IsRequired();
            builder.HasOne(cii => cii.Product).WithMany(c => c.CartItems).IsRequired();

        }
    }
}
