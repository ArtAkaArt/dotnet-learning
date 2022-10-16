using MyMigration;
using Npgsql;
using System.Data;

namespace Sol3.MyMigrationWithBlackJackAndHookers
{
    public class TableValidator
    {
        private readonly NpgsqlConnection connection;
        public TableValidator(NpgsqlConnection connection) => this.connection = connection;

         public async Task<bool> ValidateTable(string tableName)
        {
            DataTable table;

            table = await connection.GetSchemaAsync("Tables");
            var columnNames = table.AsEnumerable()
                               .Select(x => x.Field<string>("TABLE_NAME"))
                               .ToHashSet();
            if (columnNames.Contains(tableName))
                return true;
            return false;
        }
    }
}
