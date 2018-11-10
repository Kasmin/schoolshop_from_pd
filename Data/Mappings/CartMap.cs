using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Models;

namespace Shop.Data.Mappings
{
    public class CartMap : IEntityTypeConfiguration<Cart>
    {


        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasMany(c => c.CartItems);

            builder.HasData(
                new Cart() { Id = 1}
            );
        }
    }
}
