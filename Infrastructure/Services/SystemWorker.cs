using LeUs.Application.Features.Data.Commands;

namespace Leus.Infrastructure.Services;

public class SystemWorker(IServiceProvider provider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = provider.CreateScope();
            var dCheck = DateTime.UtcNow;
            dCheck = dCheck.AddHours(7);
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            if (dCheck is { Minute: < 16, Hour: 0 })
                await mediator.Send(new ReUpdateBalanceCommand(), stoppingToken);
            await Task.Delay(new TimeSpan(0, 15, 0), stoppingToken);
        }
    }
}