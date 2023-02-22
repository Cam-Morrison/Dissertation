namespace backend.services.schedulerService
{
    public class SchedulerService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(12));
        private ILogger<SchedulerService> _logger;

        public SchedulerService(ILogger<SchedulerService> logger) {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested) 
            {
                await doWorkAsync();
            }
        }

        private static async Task doWorkAsync()
        {
            MarketDataService md = new MarketDataService();
        }
    }
    
}