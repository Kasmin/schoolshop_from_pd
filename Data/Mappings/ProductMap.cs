using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Models;

namespace Shop.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.OrderItems);

            builder.HasData(
                new Product() { Id = 1, Name = "Ananas", Price = 259 },
                new Product() { Id = 2, Name = "Banan", Price = 69},
                new Product() { Id = 3, Name = "Vinograd", Price = 329},
                new Product() { Id = 4, Name = "Granat", Price = 129},
                new Product() { Id = 5, Name = "Dynya", Price = 89}
            );
        }
    }
}
