using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        builder.Property(x => x.Payload)
               .IsRequired();
        
        builder.Property(x => x.PayloadType)
               .IsRequired();
        
        builder.Property(x => x.OccuredOn)
               .IsRequired();
        
        builder.Property(x => x.LastRetryOn);
        
        builder.Property(x => x.RetryCount)
               .IsRequired();
    }
}