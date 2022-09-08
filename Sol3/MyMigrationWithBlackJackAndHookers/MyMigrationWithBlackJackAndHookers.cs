namespace MyMigration
{
    internal class MyMigrationWithBlackJackAndHookers
    {
        private readonly string connectionString;

        public MyMigrationWithBlackJackAndHookers(string connectionString) => this.connectionString = connectionString;

        internal async Task CheckDB()
        {
            var checker = new TableChecker(connectionString);
            var isFactoriesCreated = await checker.ValidateOrCreateTable("Factories");
            var isUnitsCreated =  await checker.ValidateOrCreateTable("Units");
            var isTanksCreated = await checker.ValidateOrCreateTable("Tanks");
            if (!isFactoriesCreated)
                await checker.CheckColums("Factories", new string[] { "Id", "Name", "Description" });
            if (!isUnitsCreated)
                await checker.CheckColums("Units", new string[] { "Id", "Name", "Description", "FactoryId" });
            if (!isTanksCreated)
                await checker.CheckColums("Tanks", new string[] { "Id", "Name", "Description", "Volume", "Maxvolume", "UnitId" });
        }
    }
}
