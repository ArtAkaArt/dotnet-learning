using Npgsql;
using Dapper;
using FacilityContextLib;

namespace MyMigration
{
    internal class TableCreator
    {
        private readonly NpgsqlConnection connection;
        public TableCreator(NpgsqlConnection connection) => this.connection = connection;

        internal async Task CreateFactories(Factory[] factories)
        {
            var sqlString = $"CREATE TABLE \"Factories\" (" +
                            $"\"Id\" INT GENERATED ALWAYS AS IDENTITY," +
                            $"\"Name\" varchar(50)," +
                            $"\"Description\" varchar(50)," +
                            $"PRIMARY KEY(\"Id\"));";

            var command = new NpgsqlCommand(sqlString, connection);
            await command.ExecuteNonQueryAsync();
            await FillFactories(factories);
        }
        internal async Task CreateUnits(Unit[] units)
        {
            var sqlString = "CREATE TABLE \"Units\" (" +
                            "\"Id\" INT GENERATED ALWAYS AS IDENTITY, " +
                            "\"FactoryId\" INT, " +
                            "\"Name\" varchar(50)," +
                            "\"Description\" varchar(50), " +
                            "PRIMARY KEY(\"Id\"), " +
                            "CONSTRAINT fk_factory " +
                            "FOREIGN KEY(\"FactoryId\") " +
                            "REFERENCES \"Factories\"(\"Id\") ON DELETE CASCADE);";

            var command = new NpgsqlCommand(sqlString, connection);
            await command.ExecuteNonQueryAsync();
            await FillUnits(units);
        }
        internal async Task CreateTanks(Tank[] tanks)
        {
            var sqlString = "CREATE TABLE \"Tanks\" (" +
                            "\"Id\" INT GENERATED ALWAYS AS IDENTITY, " +
                            "\"Name\" varchar(50)," +
                            "\"Volume\" INT, " +
                            "\"Maxvolume\" INT, " +
                            "\"UnitId\" INT, " +
                            "\"Description\" varchar(50), " +
                            "PRIMARY KEY(\"Id\"), " +
                            "CONSTRAINT fk_unit " +
                            "FOREIGN KEY(\"UnitId\") " +
                            "REFERENCES \"Units\"(\"Id\") ON DELETE CASCADE);";

            var command = new NpgsqlCommand(sqlString, connection);
            await command.ExecuteNonQueryAsync();
            await FillTanks(tanks);
        }
        internal async Task FillFactories(Factory[] factories)
        {
            foreach (var fact in factories)
            {
                await AddFactory(fact);
            }
        }
        internal async Task FillUnits(Unit[] units)
        {
            foreach (var unit in units)
            {
                await AddUnit(unit);
            }
        }
        internal async Task FillTanks(Tank[] tanks)
        {
            foreach (var tank in tanks)
            {
                await AddTank(tank);
            }
        }
        internal async Task AddFactory(Factory factory)
        {
            var sqlQuery = $"INSERT INTO \"Factories\" (\"Name\", \"Description\") " +
                           $"VALUES('{factory.Name}', '{factory.Description}');";
            await connection.ExecuteAsync(sqlQuery, factory);
        }
        internal async Task AddUnit(Unit unit)
        {
            var sqlQuery = $"INSERT INTO \"Units\" (\"Name\", \"Description\", \"FactoryId\") " +
                           $"VALUES('{unit.Name}', '{unit.Description}', {unit.FactoryId});";
            await connection.ExecuteAsync(sqlQuery, unit);
        }
        internal async Task AddTank(Tank tank)
        {
            var sqlQuery = $"INSERT INTO \"Tanks\" (\"Name\", \"Description\", \"Volume\", \"Maxvolume\", \"UnitId\") " +
                           $"VALUES('{tank.Name}', '{tank.Description}', {tank.Volume}, {tank.Maxvolume}, {tank.UnitId});";
            await connection.ExecuteAsync(sqlQuery, tank);
        }
    }
}