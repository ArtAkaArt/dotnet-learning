﻿using Npgsql;
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
            var tst = new Dictionary<string, Action<TItem, object>>();
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
                    var value = reader.GetValue(i);
                    if (tst.TryGetValue(reader.GetName(i), out Action<TItem, object> tst2))
                    {
                        tst2.Invoke(tItem, value);
                        continue;
                    }
                    var field = tType.GetField(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public
                                                                | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field is not null)
                    {
                        field.SetValue(tItem, value is DBNull ? null : value);
                        tst.Add(reader.GetName(i), (tItem, value) => field.SetValue(tItem, value is DBNull ? null : value));
                        continue;
                    }
                    var property = tType.GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public
                                                                      | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (property is not null)
                    {
                        property.SetValue(tItem, value is DBNull ? null : value);
                        tst.Add(reader.GetName(i), (tItem, value) => property.SetValue(tItem, value is DBNull ? null : value));
                        continue;
                    }
                    var memberWithAttr = tType.GetMembers()
                                   .Where(m => ((TableBindingNameAttribute?)m.GetCustomAttribute(typeof(TableBindingNameAttribute)))?
                                                                             .GetTableName() == reader.GetName(i))
                                   .FirstOrDefault();
                    if (memberWithAttr is null)
                        throw new InvalidCastException($"Unable to bind data from Column - {reader.GetName(i)}");
                    if (memberWithAttr.MemberType is MemberTypes.Field)
                    {
                        tst.Add(reader.GetName(i), (tItem, value) => ((FieldInfo)memberWithAttr).SetValue(tItem, value is DBNull ? null : value));
                        ((FieldInfo)memberWithAttr).SetValue(tItem, value is DBNull ? null : value);
                        continue;
                    }
                    if (memberWithAttr.MemberType is MemberTypes.Property)
                    {
                        tst.Add(reader.GetName(i), (tItem, value) => ((PropertyInfo)memberWithAttr).SetValue(tItem, value is DBNull ? null : value));
                        ((PropertyInfo)memberWithAttr).SetValue(tItem, value is DBNull ? null : value);
                    }
                }
                list.Add(tItem);
            }
            await reader.CloseAsync();
            return list;
        }
    }
}