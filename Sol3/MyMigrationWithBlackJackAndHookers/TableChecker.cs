using Npgsql;
using System.Data;
using FacilityContextLib;

namespace MyMigration
{
    internal class TableChecker
    {
        private readonly string connectionString;
        private readonly Factory[] factories;
        private readonly Unit[] units;
        private readonly Tank[] tanks;
        private readonly ColumnFixer fixer;

        public TableChecker(string connectionString)
        {
            this.connectionString = connectionString;
            var factory1 = new Factory { Id = 1, Name = "НПЗ№1", Description = "Первый нефтеперерабатывающий завод" };
            var factory2 = new Factory { Id = 2, Name = "НПЗ№2", Description = "Второй нефтеперерабатывающий завод" };
            factories = new Factory[] { factory1, factory2 };
            var unit1 = new Unit { Id = 1, Name = "ГФУ-2", Description = "Газофракционирующая установка", FactoryId = 1 };
            var unit2 = new Unit { Id = 2, Name = "АВТ-6", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 1 };
            var unit3 = new Unit { Id = 3, Name = "АВТ-10", Description = "Атмосферно-вакуумная трубчатка", FactoryId = 2 };
            units = new Unit[] { unit1, unit2, unit3 };
            var tank1 = new Tank { Id = 1, Name = "Резервуар 1", Description = "Надземный - вертикальный", Volume = 1500, Maxvolume = 2000, UnitId = 1 };
            var tank2 = new Tank { Id = 2, Name = "Резервуар 2", Description = "Надземный - горизонтальный", Volume = 2500, Maxvolume = 3000, UnitId = 1 };
            var tank3 = new Tank { Id = 3, Name = "Дополнительный резервуар 24", Description = "Надземный - горизонтальный", Volume = 3000, Maxvolume = 3000, UnitId = 2 };
            var tank4 = new Tank { Id = 4, Name = "Резервуар 35", Description = "Надземный - вертикальный", Volume = 3000, Maxvolume = 3000, UnitId = 2 };
            var tank5 = new Tank { Id = 5, Name = "Резервуар 47", Description = "Подземный - двустенный", Volume = 4000, Maxvolume = 5000, UnitId = 2 };
            var tank6 = new Tank { Id = 6, Name = "Резервуар 256", Description = "Подводный", Volume = 500, Maxvolume = 500, UnitId = 3 };
            tanks = new Tank[] { tank1, tank2, tank3, tank4, tank5, tank6 };
            fixer = new ColumnFixer(connectionString);
        }
        public async Task<bool> ValidateOrCreateTable(string tableName)
        {
            DataTable table;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                table = await connection.GetSchemaAsync("Tables");
            }
            var columnNames = table.AsEnumerable()
                               .Select(x => x.Field<string>("TABLE_NAME"))
                               .ToList();
            if (columnNames.Contains(tableName))
                return false;
            var tableCreator = new TableCreator(connectionString);
            if (tableName == "Factories")
                await tableCreator.CreateFactories(factories);
            if (tableName == "Units")
                await tableCreator.CreateUnits(units);
            if (tableName == "Tanks")
                await tableCreator.CreateTanks(tanks);
            return true;
        }
        public async Task CheckColums(string tableName, string[] columnsExpected)
        {
            DataTable table;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                table = await connection.GetSchemaAsync("Columns", new string[] { null, null, tableName }); // null, null ? требуется пояснительная бригада
                await connection.CloseAsync();
            }
            var columnNames = table.AsEnumerable()
                               .Select(x => x.Field<string>("COLUMN_NAME"))
                               .ToList();

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