using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Factors;



namespace UWPLockStep.Persistance.Configurations
{

    public class FactorBaseConfiguration : IEntityTypeConfiguration<FactorBase>
    {
        public void Configure(EntityTypeBuilder<FactorBase> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("FactorId")//Is there any benefit to not matching these to the object names
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasColumnName("FactorName");

            builder.Property(e => e.Unit).HasColumnName("FactorType");

            //builder.Property(e => e.FactorUnit).HasColumnName("FactorUnit");
        }
    }
}
