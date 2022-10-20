using Npgsql;
using System.Data;
using FacilityContextLib;

namespace MyMigration
{
    internal class ColumnChecker
    {
        private readonly NpgsqlConnection connection;
        private readonly ColumnFixer fixer;
        private readonly Factory[] factories;
        private readonly Unit[] units;
        private readonly Tank[] tanks;

        public ColumnChecker(NpgsqlConnection connection, Factory[] factories, Unit[] units, Tank[] tanks)
        {
            this.connection = connection;
            fixer = new ColumnFixer(connection);
            this.factories = factories;
            this.units = units;
            this.tanks = tanks;
        }
        public async Task CheckColumns(string tableName, string[] columnsExpected)
        {
            DataTable table;
            table = await connection.GetSchemaAsync("Columns", new string[] { null, null, tableName }); // null, null ? требуется пояснительная бригада
            var columnNames = table.AsEnumerable()
                               .Select(x => x.Field<string>("COLUMN_NAME"))
                               .ToHashSet();

            foreach (var column in columnsExpected)
            {
                if (columnNames.Contains(column))
                    continue;
                if (tableName == "Factories")
                    await fixer.AddColumn(tableName, column, factories);
                if (tableName == "Units")
                    await fixer.AddColumn(tableName, column, units);
                if (tableName == "Tanks")
                    await fixer.AddColumn(tableName, column, tanks);
            }
        }
    }
}