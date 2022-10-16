using FacilityContextLib;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using Sol3.MyMigrationWithBlackJackAndHookers;

namespace MyMigration
{
    internal class MyMigrationWithBlackJackAndHookers
    {
        private readonly string connectionString;
        private readonly Factory[] factories;
        private readonly Unit[] units;
        private readonly Tank[] tanks;

        public MyMigrationWithBlackJackAndHookers(string connectionString) 
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
        }

        internal async Task CheckDB()
        {
            var connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                await connection.OpenAsync();
                var validator = new TableValidator(connection);
                var isFactoriesExist = await validator.ValidateTable("Factories");
                var isUnitsExist = await validator.ValidateTable("Units");
                var isTanksExist = await validator.ValidateTable("Tanks");

                //зря создаю до проверок
                var tableCreator = new TableCreator(connection);
                if (!isFactoriesExist)
                    await tableCreator.CreateFactories(factories);
                if (!isUnitsExist)
                    await tableCreator.CreateUnits(units);
                if (!isTanksExist)
                    await tableCreator.CreateTanks(tanks);


                var checker = new ColumnChecker(connection, factories, units, tanks);
                if (isFactoriesExist)
                    await checker.CheckColumns("Factories", new string[] { "Id", "Name", "Description" });
                if (isUnitsExist)
                    await checker.CheckColumns("Units", new string[] { "Id", "Name", "Description", "FactoryId" });
                if (isTanksExist)
                    await checker.CheckColumns("Tanks", new string[] { "Id", "Name", "Description", "Volume", "Maxvolume", "UnitId" });
            }
        }
    }
}
