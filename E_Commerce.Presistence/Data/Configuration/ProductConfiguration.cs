using E_Commerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(N => N.Name)
                .HasMaxLength(100);
            builder.Property(D => D.Description)
                .HasMaxLength(500);
            builder.Property(P => P.PictureUrl)
                .HasMaxLength(200);
            builder.Property(P => P.Price)
                .HasPrecision(18, 2);

           builder.HasOne(P=>P.ProductBrand)
                .WithMany()
                .HasForeignKey(P=>P.BrandId);

            builder.HasOne(P => P.ProductType)
                .WithMany()
                .HasForeignKey(P => P.TypeId);
        }
    }
}
