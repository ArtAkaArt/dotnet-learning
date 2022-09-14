using Npgsql;
using System.Reflection;

namespace MyORM
{
    public static class MyORM
    {
        public async static Task<IReadOnlyCollection<TItem>> QueryMultipleItems<TItem>(this NpgsqlConnection connection, string sqlQuery)
        {
            var list = new List<TItem>();
            var command = new NpgsqlCommand(sqlQuery, connection);
            var reader = await command.ExecuteReaderAsync();
            var tType = typeof(TItem);
            if (!reader.HasRows)
            {
                await reader.CloseAsync();
                return list;
            }
            var columnsCount = reader.FieldCount;
            while (await reader.ReadAsync())
            {
                var tItem = (TItem)Activator.CreateInstance(tType)!;
                for (int i = 0; i < columnsCount; i++)
                {
                    var field = tType.GetField(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public
                                                                | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field is not null)
                    {
                        field.SetValue(tItem, reader.GetValue(i));
                        continue;
                    }
                    var property = tType.GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public
                                                                      | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (property is not null)
                    {
                        property.SetValue(tItem, reader.GetValue(i));
                        continue;
                    }
                    var memberWithAttr = tType.GetMembers()
                                   .Where(m => ((TableBindingNameAttribute?)m.GetCustomAttribute(typeof(TableBindingNameAttribute)))?
                                                                             .GetTableName() == reader.GetName(i))
                                   .FirstOrDefault();
                    if (memberWithAttr is null )
                        throw new InvalidCastException($"Unable to bind data from Column - {reader.GetName(i)}");
                    if (memberWithAttr.MemberType is MemberTypes.Field)
                        ((FieldInfo)memberWithAttr).SetValue(tItem, reader.GetValue(i));
                    if (memberWithAttr.MemberType is MemberTypes.Property)
                        ((PropertyInfo)memberWithAttr).SetValue(tItem, reader.GetValue(i));
                }
                list.Add(tItem);
            }
            await reader.CloseAsync();
            return list;
        }
    }
}