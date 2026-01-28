using Microsoft.EntityFrameworkCore;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public interface IEventOutboxDbContext
{
    DbSet<Event> Events { get; }
}