using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Orders;

namespace UWPLockStep.Persistance.Configurations.Orders
{
    public class OrderBaseConfiguration : IEntityTypeConfiguration<OrderBase>
    {
        public void Configure(EntityTypeBuilder<OrderBase> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasColumnName("OrderId")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Name).HasColumnName("OrderName");

            builder.Property(o => o.Description).HasColumnName("OrderDescription");

            //builder.Property(o => o.PrescriberNotes).HasColumnName("PrescriberNotes");

            //builder.HasMany(o => o.OrderPolicies)
            //    .WithOne(o => o.Order);

            //builder.HasOne(o => o.Prescriber).WithMany(o => o.Orders).HasForeignKey(o => o.PrescriberId);
        }
    }
}
