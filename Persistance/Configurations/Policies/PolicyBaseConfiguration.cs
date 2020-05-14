using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Policies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UWPLockStep.Persistance.Configurations
{
    public class PolicyBaseConfiguration : IEntityTypeConfiguration<PolicyBase>
    {
        public void Configure(EntityTypeBuilder<PolicyBase> builder)
        {
            builder.HasKey(p => p.PolicyId);
            builder.Property(e => e.PolicyId).HasColumnName("PolicyId")
                .IsRequired()
                .ValueGeneratedOnAdd();
           // builder.Property(e => e.PolicyDuration).HasConversion<long>().HasColumnName("PolicyDuration");
            // builder.Property(p => p.PolicyBasis).HasColumnName("PolicyBasis");//mover to Decorator
            //builder.Property(p => p.WarningLevel).HasColumnName("WarningLevel");

            builder.Property(e => e.Guidance)
                .HasConversion<string>()
                .HasColumnName("PolicyGuidance");
            //.HasConversion(v => v.ToString(), v => (PolicyGuidance)Enum.Parse(typeof(PolicyGuidance), v)) //this is the manual way to configure the above conversion

            //builder.Property(p => p.PatientType).HasColumnName("PatientType");
            


        }

    }
}
