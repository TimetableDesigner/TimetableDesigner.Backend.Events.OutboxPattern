using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public class EventOutboxSender<TDbContext> : BackgroundService where TDbContext : DbContext, IEventOutboxDbContext
{
    private readonly TDbContext  _databaseContext;
    private readonly IEventQueuePublisher _eventQueuePublisher;
    
    public EventOutboxSender(TDbContext databaseContext, IEventQueuePublisher eventQueuePublisher)
    {
        _databaseContext = databaseContext;
        _eventQueuePublisher = eventQueuePublisher;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Event? eventData = await _databaseContext.Events.FirstOrDefaultAsync(x => x.LastRetryOn == null, stoppingToken)
                            ?? await _databaseContext.Events.OrderBy(x => x.LastRetryOn).FirstOrDefaultAsync(stoppingToken);

            if (eventData is null)
            {
                continue;
            }
            
            Type payloadType = Type.GetType(eventData.PayloadType)!;
            JsonSerializer.Deserialize(eventData.Payload, payloadType);

            try
            {
                await _eventQueuePublisher.PublishAsync(eventData.Payload, payloadType);
                    
                _databaseContext.Events.Remove(eventData);
            }
            catch
            {
                eventData.LastRetryOn = DateTimeOffset.UtcNow;
                eventData.RetryCount++;
            }
            finally
            {
                await _databaseContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}