using Npgsql;

namespace MyMigration
{
    internal class MyMigrationWithBlackJackAndHookers
    {
        private readonly NpgsqlConnection connection;

        public MyMigrationWithBlackJackAndHookers(string connectionString) => connection = new NpgsqlConnection(connectionString);

        internal async Task CheckDB()
        {
            using (connection)
            {
                await connection.OpenAsync();
                var checker = new TableChecker(connection);
                var isFactoriesCreated = await checker.ValidateOrCreateTable("Factories");
                var isUnitsCreated = await checker.ValidateOrCreateTable("Units");
                var isTanksCreated = await checker.ValidateOrCreateTable("Tanks");
                if (!isFactoriesCreated)
                    await checker.CheckColumns("Factories", new string[] { "Id", "Name", "Description" });
                if (!isUnitsCreated)
                    await checker.CheckColumns("Units", new string[] { "Id", "Name", "Description", "FactoryId" });
                if (!isTanksCreated)
                    await checker.CheckColumns("Tanks", new string[] { "Id", "Name", "Description", "Volume", "Maxvolume", "UnitId" });
            }
        }
    }
}
