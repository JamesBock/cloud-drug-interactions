using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Policies;

namespace UWPLockStep.Persistance.Configurations
{
    public class IngredientBaseConfiguration :IEntityTypeConfiguration<IngredientBase>
        {
            public void Configure(EntityTypeBuilder<IngredientBase> builder)
            {
                builder.HasKey(i => i.Id);
            
                builder.Property(i => i.Id).HasColumnName("IngredientId")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                builder.Property(i => i.Name).HasColumnName("IngredientName");

                //builder.Property(i => i.IngredientUnit).HasColumnName("IngredientUnit");//Replaced but Unit Classes


            }
        }
}
