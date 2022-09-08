namespace MyMigration
{
    internal class MyMigrationWithBlackJackAndHookers
    {
        private readonly string connectionString;

        public MyMigrationWithBlackJackAndHookers(string connectionString) => this.connectionString = connectionString;

        internal async Task CheckDB()
        {
            var cheker = new TableChecker(connectionString);
            var isFactoriesCreated = await cheker.ValidateTable("Factories");
            var isUnitsCreated =  await cheker.ValidateTable("Units");
            var isTanksCreated = await cheker.ValidateTable("Tanks");
            if (!isFactoriesCreated)
                await cheker.CheckColums("Factories", new string[] { "Id", "Name", "Description" });
            if (!isUnitsCreated)
                await cheker.CheckColums("Units", new string[] { "Id", "Name", "Description", "FactoryId" });
            if (!isTanksCreated)
                await cheker.CheckColums("Tanks", new string[] { "Id", "Name", "Description", "Volume", "Maxvolume", "UnitId" });
        }
    }
}
