using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public class EventOutboxSender<TDbContext> : BackgroundService where TDbContext : DbContext, IEventOutboxDbContext
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEventQueuePublisher _eventQueuePublisher;
    
    public EventOutboxSender(IServiceScopeFactory serviceScopeFactory, IEventQueuePublisher eventQueuePublisher)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _eventQueuePublisher = eventQueuePublisher;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        using (TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>())
        {
            await dbContext.Database.EnsureCreatedAsync(stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                Event? eventData = await dbContext.Events.FirstOrDefaultAsync(x => x.LastRetryOn == null, stoppingToken)
                                ?? await dbContext.Events.OrderBy(x => x.LastRetryOn).FirstOrDefaultAsync(stoppingToken);

                if (eventData is null)
                {
                    continue;
                }
            
                Type payloadType = Type.GetType(eventData.PayloadType)!;
                JsonSerializer.Deserialize(eventData.Payload, payloadType);

                try
                {
                    await _eventQueuePublisher.PublishAsync(eventData.Payload, payloadType);
                    
                    dbContext.Events.Remove(eventData);
                }
                catch
                {
                    eventData.LastRetryOn = DateTimeOffset.UtcNow;
                    eventData.RetryCount++;
                }
                finally
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}