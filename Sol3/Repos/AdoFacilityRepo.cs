using Npgsql;
using Dapper;
using System.Data;
using FacilityContextLib;
using Sol3.Profiles;

namespace Sol3.Repos
{
    public class AdoFacilityRepo : IMyRepo
    {
        //private IDbConnection connection;
        private string connectionString;
        public AdoFacilityRepo(string connectionString) => this.connectionString = connectionString;
        public async Task<List<Unit>> GetAllUnits()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            return (await connection.QueryAsync<Unit>("SELECT * FROM \"Units\"")).ToList();
        }
        public async Task<Unit> GetUnitById(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
#pragma warning disable CS8603 // Possible null reference return.
            return (await connection.QueryAsync<Unit>($"SELECT * FROM \"Units\" " +
                                                      $"WHERE \"Id\" = {id}")).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
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
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO \"Units\" (\"Name\", \"Description\", \"FactoryId\") " +
                               $"VALUES('{unit.Name}', '{unit.Description}', {unit.FactoryId});";
                await connection.ExecuteAsync(sqlQuery, unit);
            }
            return unit;
        }

        public async Task<Unit> UpdateUnit(Unit unit, UnitDTO unitUpd)
        {
            if (unit is null)
                throw new Exception($"Unit не найден");
            unit.Id = unitUpd.Id;
            unit.Name = unitUpd.Name;
            unit.Description = unitUpd.Description;
            unit.FactoryId = unitUpd.Factoryid;
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE \"Units\" " +
                               $"SET \"Name\" = '{unit.Name}', \"Description\" = '{unit.Description}', \"FactoryId\" = {unit.FactoryId} " +
                               $"WHERE \"Id\" = {unit.Id}";
                await connection.ExecuteAsync(sqlQuery, unit);
            }
            return unit;
        }
        public async Task DeleteUnitById(Unit unit)
        {
            if (unit is null)
                throw new Exception($"Unit не найден");
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM \"Units\" " +
                               $"WHERE \"Id\" = {unit.Id}";
                await connection.ExecuteAsync(sqlQuery);
            }
        }
        public async Task<List<Tank>> GetAllTanks()
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
            return (await connection.QueryAsync<Tank>("SELECT * FROM \"Tanks\"")).ToList();
        }
        public async Task<Tank> GetTankById(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
#pragma warning disable CS8603 // Possible null reference return.
            return (await connection.QueryAsync<Tank>($"SELECT * FROM \"Tanks\" " +
                                                      $"WHERE \"Id\" = {id}"))
                                                      .FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
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
            using (var connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"INSERT INTO \"Tanks\" (\"Name\", \"Description\", \"Volume\", \"Maxvolume\", \"UnitId\") " +
                               $"VALUES('{tank.Name}', '{tank.Description}', {tank.Volume}, {tank.Maxvolume}, {tank.UnitId});";
                await connection.ExecuteAsync(sqlQuery, tank);
            }
            return tank;
        }
        public async Task<Tank> UpdateTank(Tank tank, TankDTO tankUpd)
        {
            if (tank is null)
                throw new Exception($"Tank не найден");
            tank.Id = tankUpd.Id;
            tank.Name = tankUpd.Name;
            tank.Description = tankUpd.Description;
            tank.Volume = tankUpd.Volume;
            tank.Maxvolume = tankUpd.Maxvolume;
            tank.UnitId = tankUpd.Unitid;

            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"UPDATE \"Tanks\" " +
                               $"SET \"Name\" = '{tank.Name}', \"Description\" = '{tank.Description}', \"Volume\" = {tank.Volume}, " +
                                    $"\"Maxvolume\" = {tank.Maxvolume}, \"UnitId\" = {tank.UnitId}" +
                               $"WHERE \"Id\" = {tank.Id}";
                await connection.ExecuteAsync(sqlQuery, tank);
            }
            return tank;
        }
        public async Task DeleteTankById(Tank tank)
        {
            if (tank is null)
                throw new Exception($"Tank не найден");
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = $"DELETE FROM \"Tanks\" " +
                               $"WHERE \"Id\" = {tank.Id}";
                await connection.ExecuteAsync(sqlQuery);
            }
        }
        public async Task<Factory> GetFactoryById(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(connectionString);
#pragma warning disable CS8603 // Possible null reference return.
            return (await connection.QueryAsync<Factory>($"SELECT * FROM \"Factories\" " +
                                                      $"WHERE \"Id\" = {id}")).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
