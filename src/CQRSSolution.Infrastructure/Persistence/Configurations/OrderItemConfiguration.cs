using CQRSSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRSSolution.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.OrderItemId);
        
        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(oi => oi.UnitPrice)
             .HasColumnType("decimal(18,2)");
    }
}
