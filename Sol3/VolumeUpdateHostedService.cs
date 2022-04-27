using AutoMapper;
using Sol3.Profiles;

namespace Sol3
{
    public class VolumeUpdateHostedService : BackgroundService
    {
        private readonly ILogger<VolumeUpdateHostedService> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Random rnd;
        private readonly IMapper mapper;
        private readonly IHostApplicationLifetime lifetime;

        public VolumeUpdateHostedService(ILogger<VolumeUpdateHostedService> logger, IServiceScopeFactory scopeFactory, IMapper mapper, IHostApplicationLifetime lifetime)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
            rnd = new();
            this.mapper = mapper;
            this.lifetime = lifetime;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await WaitForAppStartup(lifetime, stoppingToken))
                return;
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<FacilityRepo>();
                    var tanks = await repo.GetAllTanks();
                    //logger.LogInformation("Начало foreach---------------------------------------");
                    foreach (var tank in tanks)
                    {
                        var tankUpd = mapper.Map<TankDTO>(tank);
                        var modification = rnd.Next(-10, 10);
                        //logger.LogInformation($"Volume до изменения = {tankUpd.Volume} 10% = {modification}");
                        tankUpd.Volume += (int)(tankUpd.Volume / 100.0 * modification);
                        //logger.LogInformation($"Измененный Volume = {tankUpd.Volume} 10% = {modification}");
                        if (tankUpd.Volume > tankUpd.Maxvolume || tankUpd.Volume < 0)
                            logger.LogError($"Првевышение предела Volume {tankUpd.Volume} больше чем MaxVolume {tankUpd.Maxvolume} или меньше нуля");
                        else await repo.UpdateTank(tank, tankUpd);
                    }
                }
                await Task.Delay(600_000, stoppingToken);
            }
        }
        //честно стырил с хабра, но разобрался зачем оно нужно
        static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
        {
            var startedSource = new TaskCompletionSource();
            using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

            var cancelledSource = new TaskCompletionSource();
            using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

            var completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

            return completedTask == startedSource.Task;
        } 
    }
}
