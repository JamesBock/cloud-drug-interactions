using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Policies;


namespace UWPLockStep.Persistance.Configurations
{
    public class FactorConfiguration : IEntityTypeConfiguration<Factor>
    {
        public void Configure(EntityTypeBuilder<Factor> builder)
        {
            //builder.HasKey(e => e.FactorId);
                
            //builder.Property(e => e.FactorId).HasColumnName("FactorId")
            //    .IsRequired()
            //    .ValueGeneratedOnAdd();
            
            //builder.Property(e => e.FactorName).HasColumnName("FactorName");
            
            //builder.Property(e => e.FactorType).HasColumnName("FactorType");

            //builder.Property(e => e.FactorUnit).HasColumnName("FactorUnit");

            //builder.HasMany(e => e.FactorPolicies)
            //    .WithOne(e => e.Factor);
                //.HasForeignKey(p => p.PolicyId); Throws Exception
                //Reomved Policy fomr the Factor object and configured relationship in Policy class
        }
    }
}
