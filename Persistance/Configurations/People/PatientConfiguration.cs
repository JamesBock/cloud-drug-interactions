using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Entities.Policies;

namespace UWPLockStep.Persistance.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<LockStepPatient>
    {
        public void Configure(EntityTypeBuilder<LockStepPatient> builder)
        {
            builder.HasKey(p => p.LockStepId);
            builder.Property(p => p.LockStepId).HasColumnName("PatientId")
                  .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(p => p.GivenNames).HasColumnName("FirstName");
            builder.Property(p => p.LastName).HasColumnName("LastName");
            builder.Property(p => p.DateOfBirth).HasColumnName("DateOfBirth");
            builder.Property(p => p.DayOfLife).HasColumnName("DayOfLife");
            //builder.Property(p => p.PatientType).HasColumnName("PatientType");
            builder.Property(p => p.Weight).HasColumnName("Weight");
            builder.HasOne(p => p.Practitioner).WithMany(p => p.Patients);
        }
    }
}
