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
    
    public static Event Create<T>(T payload) where T : class => new Event
    {
        Payload = JsonSerializer.Serialize<T>(payload),
        PayloadType = payload.GetType().FullName!
    };
}
