using System.Text.Json;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public class Event
{
    public Guid Id { get; set; }
    public required string Payload { get; set; }
    public required string PayloadType { get; set; }
    public DateTimeOffset OccuredOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastRetryOn { get; set; }
    public uint RetryCount { get; set; }
    
    public Event(object payload)
    {
        Payload = JsonSerializer.Serialize(payload);
        PayloadType = payload.GetType().FullName!;
    }
}