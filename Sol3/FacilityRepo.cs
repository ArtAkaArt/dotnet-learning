using FacilityContextLib;
using Microsoft.EntityFrameworkCore;
using Sol3.Profiles;

namespace Sol3
{
    public class FacilityRepo
    {
        private readonly FacilityContext context;
        ILogger<FacilityRepo> logger;
        public FacilityRepo(FacilityContext context, ILogger<FacilityRepo> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task<List<Unit>> GetAllUnits()
        {
            return await context.Units.ToListAsync(); 
        }
        public async Task<Unit> GetUnitById(int id)
        {
            return await context.Units.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Unit> AddUnit(UnitShort us)
        {
            var unit = new Unit
            {
                Id = 0,
                Name = us.Name,
                Description = us.Description,
                FactoryId = us.Factoryid
            };
            context.Units.Add(unit);
            await context.SaveChangesAsync();
            return unit;
        }
        public async Task ReplaceUnitById(int id, UnitDTO unitUpd)
        {
            var unit = await context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit != null)
            {
                unit.Id = unitUpd.Id;
                unit.Name = unitUpd.Name;
                unit.Description = unitUpd.Description;
                unit.FactoryId = unitUpd.Factoryid;
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteUnitById(int id)
        {
            var unit = await context.Units.FirstOrDefaultAsync(x => x.Id == id);
            if (unit != null)
            {
                context.Units.Remove(unit);
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Юнит с Id {id} не найден"); // кстати инетерсно, считать ли ошибкой если нет юнита, который надо удалить?
        }
        public async Task<Tank> GetTankById(int id)
        {
            return await context.Tanks.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Tank> AddTank(int unitId, TankShort ts) //* как и в юните добавлять с пустым id пока нельзя, присваивания в базе не проиходит?
        {
            var tank = new Tank()
            {
                Name = ts.Name,
                Description = ts.Description,
                Volume = ts.Volume,
                Maxvolume = ts.Maxvolume,
                UnitId = unitId,
            };
            context.Tanks.Add(tank);
            await context.SaveChangesAsync();
            return tank;
        }
        public async Task ReplaceTankById(int id, TankDTO tankUpd)
        {
            var tank = await context.Tanks.FirstOrDefaultAsync(x => x.Id == id);
            /* не уверен какая именно логика тут нужна будет по заданию, но я бы пока так сделал
             * if (tankUpd.Maxvolume<tankUpd.Volume || 0> tankUpd.Volume)   MaxVolume беру у апдейта, если вдруг он там тоже меняется
             * {
             *      logger.LogError($"Значение Volume {tankUpd.Volume} выходит за допустимый предел");
             *      throw new Exception($"Значение Volume {tankUpd.Volume} выходит за допустимый предел"); не уверен что надо в ошибку выходить, но пусть
             * }
             */

            if (tank != null)
            {
                tank.Id = tankUpd.Id;
                tank.Name = tankUpd.Name;
                tank.Description = tankUpd.Description;
                tank.Volume = tankUpd.Volume;
                tank.Maxvolume = tankUpd.Maxvolume;
                tank.UnitId = tankUpd.Unitid;
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Танк с Id {id} не найден");
        }
        public async Task DeleteTankById(int id)
        {
            var tank = await context.Tanks.FirstOrDefaultAsync(x => x.Id == id);
            if (tank != null)
            {
                context.Tanks.Remove(tank);
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Танк с Id {id} не найден");
        }
        public async Task<Factory> GetFactoryById(int id)
        {
            return await context.Factories.Include( x => x.Units).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
