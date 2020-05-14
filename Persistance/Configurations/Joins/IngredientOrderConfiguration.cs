using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Policies;

using UWPLockStep.Domain.Entities.Joins;

namespace UWPLockStep.Persistance.Configurations
{
    public class IngredientOrderConfiguration : IEntityTypeConfiguration<IngredientOrder>
    {
        public void Configure(EntityTypeBuilder<IngredientOrder> builder)
        {
            builder.HasKey(o => new { o.OrderId, o.IngredientId });

            builder.Property(e => e.OrderId).HasColumnName("OrderId");

            builder.Property(i => i.IngredientId).HasColumnName("IngredientId");

            builder.HasOne(d => d.Order)
                .WithMany(p => p.IngredientOrders)
                .HasForeignKey(d => d.OrderId); //.OnDelete(DeleteBehavior.ClientSetNull) //this is included in the Northwind Config example 

            builder.HasOne(d => d.Ingredient)
              .WithMany(p => p.IngredientOrders)
              .HasForeignKey(d => d.IngredientId);
        }
    }
}
