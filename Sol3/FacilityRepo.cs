﻿using FacilityContextLib;
using Microsoft.EntityFrameworkCore;
using Sol3.Controllers;
using static Sol3.Controllers.TanksApi;

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
                context.Units.Update(unit);
                await context.SaveChangesAsync();
            }
            else throw new Exception($"Юнит с Id {id} не найден");
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
            if (tank != null)
            {
                tank.Id = tankUpd.Id;
                tank.Name = tankUpd.Name;
                tank.Description = tankUpd.Description;
                tank.Volume = tankUpd.Volume;
                tank.Maxvolume = tankUpd.Maxvolume;
                tank.UnitId = tankUpd.Unitid;
                context.Tanks.Update(tank);
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
