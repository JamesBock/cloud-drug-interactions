using System;
using System.Collections.Generic;
using System.Text;
using UWPLockStep.Domain.Entities.Policies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UWPLockStep.Domain.Entities.Orders;

namespace UWPLockStep.Persistance.Configurations
{
    public class OrderPolicyConfiguration : IEntityTypeConfiguration<OrderPolicy>
    {
        public void Configure(EntityTypeBuilder<OrderPolicy> builder)
        {
            builder.HasOne<Order>()
                 .WithMany()
                 .HasForeignKey(p => p.OrderId);
        }
    }
}
