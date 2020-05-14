using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Intermediates;

namespace UWPLockStep.Persistance.Configurations.Intermediates
{
    class FactorItemConfiguration : IEntityTypeConfiguration<FactorItem>
    {
        public void  Configure(EntityTypeBuilder<FactorItem> builder)
        {
            //builder.HasOne(fi => fi.Ingredient)
            //    .WithOne(i => i.)
                
        }
    }
}
