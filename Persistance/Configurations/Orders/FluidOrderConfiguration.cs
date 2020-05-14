using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Orders;



namespace UWPLockStep.Persistance.Configurations
{
    public class FluidOrderConfiguration : IEntityTypeConfiguration<FluidOrder>
    {
        public void Configure(EntityTypeBuilder<FluidOrder> builder)
        {
            //builder.Property(o => o.Volume).HasColumnName("Volume");
            //builder.Property(o => o.Duration).HasConversion<long>().HasColumnName("Duration");
            //builder.Property(o => o.AdministrationRoute).HasColumnName("AdimnistrationRoute");
            
        }
    }
}
