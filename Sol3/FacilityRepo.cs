using FacilityContextLib;
using Microsoft.EntityFrameworkCore;
using Sol3.Profiles;

namespace Sol3
{
    public class FacilityRepo
    {
        private readonly FacilityContext context;
        public FacilityRepo(FacilityContext context)
        {
            this.context = context;
        }
        public async Task<List<Unit>> GetAllUnits()
        {
            return await context.Units.ToListAsync(); 
        }
        public async Task<Unit> GetUnitById(int id)
        {
            return await context.Units.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Unit> AddUnit(CreateUnitDTO us)
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
        public async Task<Unit> UpdateUnit(Unit unit, UnitDTO unitUpd)
        {
            if (unit != null)
            {
                unit.Id = unitUpd.Id;
                unit.Name = unitUpd.Name;
                unit.Description = unitUpd.Description;
                unit.FactoryId = unitUpd.Factoryid;
                await context.SaveChangesAsync();
                return unit;
            }
            else throw new Exception($"Unit не найден");
        }
        public async Task DeleteUnitById(Unit unit)
        {
            if (unit != null)
            {
                context.Units.Remove(unit);
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Unit не найден");
        }
        public async Task<List<Tank>> GetAllTanks()
        {
            return await context.Tanks.ToListAsync();
        }
        public async Task<Tank> GetTankById(int id)
        {
            return await context.Tanks.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Tank> AddTank(int unitId, TankDTO ts)
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
        public async Task<Tank> UpdateTank(Tank tank, TankDTO tankUpd)
        {
            if (tank != null)
            {
                tank.Id = tankUpd.Id;
                tank.Name = tankUpd.Name;
                tank.Description = tankUpd.Description;
                tank.Volume = tankUpd.Volume;
                tank.Maxvolume = tankUpd.Maxvolume;
                tank.UnitId = tankUpd.Unitid;
                await context.SaveChangesAsync();
                return tank;
            }
            else throw new Exception($"Tank не найден");
        }
        public async Task DeleteTankById(Tank tank)
        {
            if (tank != null)
            {
                context.Tanks.Remove(tank);
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Tank не найден");
        }
        public async Task<Factory> GetFactoryById(int id)
        {
            return await context.Factories.Include( x => x.Units).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
