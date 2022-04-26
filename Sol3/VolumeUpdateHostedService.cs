using AutoMapper;
using Sol3.Profiles;

namespace Sol3
{
    public class VolumeUpdateHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<VolumeUpdateHostedService> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private Timer timer = null!;
        private Random rnd;
        private readonly IMapper mapper;

        public VolumeUpdateHostedService(ILogger<VolumeUpdateHostedService> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
            rnd = new();
            this.mapper = mapper;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            timer = new Timer(VolumesUpdateAsync, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }
        private async void VolumesUpdateAsync(object? state)
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
                    if (modification != 0)
                    {
                        //logger.LogInformation($"Volume до изменения = {tankUpd.Volume} 10% = {modification}");
                        tankUpd.Volume += (int) (tankUpd.Volume /100.0 * modification);
                        //logger.LogInformation($"Измененный Volume = {tankUpd.Volume} 10% = {modification}");
                    }
                    if (tankUpd.Volume > tankUpd.Maxvolume || tankUpd.Volume < 0)
                        logger.LogError("Првевышение предела Volume");
                        else await repo.UpdateTank(tank, tankUpd);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
