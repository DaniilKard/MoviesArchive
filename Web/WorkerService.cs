namespace MoviesArchive.Web;

public class WorkerService : BackgroundService
{
    public ILogger Logger { get; }

    public WorkerService(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger<WorkerService>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("WorkerService is starting.");

        stoppingToken.Register(() => Logger.LogInformation("WorkerService is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            Logger.LogInformation("WorkerService is doing background work.");

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }

        Logger.LogInformation("WorkerService has stopped.");
    }
}
