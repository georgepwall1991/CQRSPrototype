using CQRSSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRSSolution.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(om => om.Id);
        
        builder.Property(om => om.Type)
            .IsRequired();
            
        builder.Property(om => om.Payload)
            .IsRequired();
            
        builder.Property(om => om.Attempts)
            .HasDefaultValue(0);
    }
}
