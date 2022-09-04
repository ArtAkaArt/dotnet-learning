using FacilityContextLib;
using Sol3.Profiles;

namespace Sol3.Repos
{
    public interface IMyRepo
    {
        public abstract Task<List<Unit>> GetAllUnits();
        public abstract Task<Unit> GetUnitById(int id);
        public abstract Task<Unit> AddUnit(CreateUnitDTO us);
        public abstract Task<Unit> UpdateUnit(Unit unit, UnitDTO unitUpd);
        public abstract Task DeleteUnitById(Unit unit);
        public abstract Task<List<Tank>> GetAllTanks();
        public abstract Task<Tank> GetTankById(int id);
        public abstract Task<Tank> AddTank(int unitId, TankDTO ts);
        public abstract Task<Tank> UpdateTank(Tank tank, TankDTO tankUpd);
        public abstract Task DeleteTankById(Tank tank);
        public abstract Task<Factory> GetFactoryById(int id);
    }
}
