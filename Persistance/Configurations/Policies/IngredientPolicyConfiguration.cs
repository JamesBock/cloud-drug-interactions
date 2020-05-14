using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Policies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UWPLockStep.Domain.Entities.Ingredients;

namespace UWPLockStep.Persistance.Configurations
{
    public class IngredientPolicyConfiguration : IEntityTypeConfiguration<IngredientPolicy>
    {
        public void Configure(EntityTypeBuilder<IngredientPolicy> builder)
        {
            builder.Property(p => p.Minimum).HasColumnName("Minimum");
            builder.Property(p => p.Maximum).HasColumnName("Maximum");
            builder.HasOne<Ingredient>() //Refactored Ingredient to remove Policy collection...was NavigationProperty...
               .WithMany()
               .HasForeignKey(p => p.IngredientId);
        }
    }
}
