using Npgsql;
using FacilityContextLib;

namespace MyMigration
{
    internal class ColumnFixer
    {
        private readonly NpgsqlConnection connection;
        internal ColumnFixer(NpgsqlConnection connection) => this.connection = connection;
        internal async Task AddColumn(string tableName, string columnName, IFacility[] facilities)
        {
            var alterStringColumn = $"ALTER TABLE \"{tableName}\"" +
                                   $"ADD COLUMN \"{columnName}\" varchar(50);";
            var alterIntColumn = $"ALTER TABLE \"{tableName}\"" +
                                $"ADD COLUMN \"{columnName}\" INT;";
            switch (columnName)
            {
                case "Name":

                    var command = new NpgsqlCommand(alterStringColumn, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var facility in facilities)
                    {
                        await UpdateStringValue(tableName, columnName, facility.Id, facility.Name);
                    }
                    break;
                case "Description":

                    command = new NpgsqlCommand(alterStringColumn, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var facility in facilities)
                    {
                        await UpdateStringValue(tableName, columnName, facility.Id, facility.Description);
                    }
                    break;
                case "Volume":

                    command = new NpgsqlCommand(alterIntColumn, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var tank in (Tank[])facilities)
                    {
                        await UpdateIntValue(tableName, columnName, tank.Id, tank.Volume);
                    }
                    break;
                case "Maxvolume":
                    command = new NpgsqlCommand(alterIntColumn, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var tank in (Tank[])facilities)
                    {
                        await UpdateIntValue(tableName, columnName, tank.Id, tank.Maxvolume);
                    }
                    break;
                case "UnitId":

                    var sqlString = $"ALTER TABLE \"{tableName}\"" +
                                    $"ADD COLUMN \"{columnName}\"Int," +
                                    "ADD CONSTRAINT fk_unit " +
                                    "FOREIGN KEY(\"UnitId\") " +
                                    "REFERENCES \"Units\"(\"Id\") ON DELETE CASCADE;";
                    command = new NpgsqlCommand(sqlString, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var tank in (Tank[])facilities)
                    {
                        await UpdateIntValue(tableName, columnName, tank.Id, tank.UnitId);
                    }
                    break;
                case "FactoryId":

                    sqlString = $"ALTER TABLE \"{tableName}\"" +
                                    $"ADD COLUMN \"{columnName}\"Int," +
                                    "ADD CONSTRAINT fk_factory " +
                                    "FOREIGN KEY(\"FactoryId\") " +
                                    "REFERENCES \"Factories\"(\"Id\") ON DELETE CASCADE;";
                    command = new NpgsqlCommand(sqlString, connection);
                    await command.ExecuteNonQueryAsync();
                    foreach (var unit in (Unit[])facilities)
                    {
                        await UpdateIntValue(tableName, columnName, unit.Id, unit.FactoryId);
                    }
                    break;
                default:
                    break;
            }
        }
        internal async Task UpdateIntValue(string tableName, string columnName, int id, int value)
        {
            var sqlString = $"UPDATE \"{tableName}\" " +
                            $"SET \"{columnName}\" = {value}" +
                            $"WHERE \"Id\" = {id};";
            var command = new NpgsqlCommand(sqlString, connection);
            await command.ExecuteNonQueryAsync();
        }
        internal async Task UpdateStringValue(string tableName, string columnName, int id, string value)
        {
            var sqlString = $"UPDATE \"{tableName}\" " +
                            $"SET \"{columnName}\" = '{value}'" +
                            $"WHERE \"Id\" = {id};";
            var command = new NpgsqlCommand(sqlString, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}