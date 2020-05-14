using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.Domain.Entities.Joins;

namespace UWPLockStep.Persistance.Configurations
{
    public class FactorIngredientConfiguration : IEntityTypeConfiguration<FactorIngredient>
    {
        public void Configure(EntityTypeBuilder<FactorIngredient> builder)
        {
            builder.HasKey(e => new { e.FactorId, e.IngredientId });

            builder.Property(e => e.FactorId).HasColumnName("FactorId");

            builder.Property(i => i.IngredientId).HasColumnName("IngredientId");

            builder.HasOne(d => d.Factor)
                .WithMany(p => p.FactorIngredients)
                .HasForeignKey(d => d.FactorId); //.OnDelete(DeleteBehavior.ClientSetNull) //this is included in the Northwind Config example 

            builder.HasOne(d => d.Ingredient)
              .WithMany(p => p.FactorIngredients)
              .HasForeignKey(d => d.IngredientId);


        }
    }
}
