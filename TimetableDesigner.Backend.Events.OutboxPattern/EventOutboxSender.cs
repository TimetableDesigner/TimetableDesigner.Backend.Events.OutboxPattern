using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TimetableDesigner.Backend.Events.OutboxPattern;

public class EventOutboxSender<TDbContext> : BackgroundService where TDbContext : DbContext, IEventOutboxDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEventQueuePublisher _eventQueuePublisher;
    
    public EventOutboxSender(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IEventQueuePublisher eventQueuePublisher)
    {
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
        _eventQueuePublisher = eventQueuePublisher;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IConfigurationSection section = _configuration.GetSection("Workers:EventOutboxSender");
        int delayBetweenQueries = section.GetValue("DelayBetweenEmptyQueries", 10);
        int limitInSingleQuery = section.GetValue("LimitInSingleQuery", 50);
        
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        using (TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>())
        {
            await dbContext.Database.EnsureCreatedAsync(stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                IAsyncEnumerable<Event>? events = dbContext.Events
                                                           .OrderBy(x => x.LastRetryOn)
                                                           .Take(limitInSingleQuery)
                                                           .ToAsyncEnumerable();

                bool any = false;
                await foreach (Event eventData in events)
                {
                    any = true;
                    
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
                }
                
                if (!any)
                {
                    await Task.Delay(TimeSpan.FromSeconds(delayBetweenQueries), stoppingToken);
                }
                else
                {
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}