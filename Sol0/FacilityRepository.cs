using Npgsql;
using static Sol0.UserInterractions;
using static Sol0.Program;

namespace Sol0
{
    internal class FacilityRepository
    {
        private readonly string conn_param;
        internal FacilityRepository(Account acc)
        {
            conn_param = $"Server=127.0.0.1;Port=5432;User Id={acc.AccName};Password={acc.Password};Database=Facilities;";
        }
        internal async Task CreateUnit()//string conn_param
        {
            using var conn = new NpgsqlConnection(conn_param);
            if (ReadUnitInput(out InputContainer stats))
            {
                conn.Open();
                await using (var comm = new NpgsqlCommand($"INSERT INTO units (id, name, factoryid) VALUES (@i1, @n1, @f1)", conn))
                {
                    comm.Parameters.AddWithValue("i1", stats.Id);
                    comm.Parameters.AddWithValue("n1", stats.Name);
                    comm.Parameters.AddWithValue("f1", stats.FactoryId);

                    int nRows = await comm.ExecuteNonQueryAsync();
                    //Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }
                conn.Close();
            }
        }
        internal async Task UpdateUnit()
        {
            var list = await ReadFacilities();
            list.ForEach(t => t.PrintInfo());
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int idDel);
            if (ReadUnitInput(out InputContainer stats))
            {
                using var conn = new NpgsqlConnection(conn_param);
                conn.Open();
                await using (var comm = new NpgsqlCommand("UPDATE units SET id = @i, name = @n, factoryid = @f WHERE id = @d", conn))
                {

                    comm.Parameters.AddWithValue("i", stats.Id);
                    comm.Parameters.AddWithValue("n", stats.Name);
                    comm.Parameters.AddWithValue("f", stats.FactoryId);
                    comm.Parameters.AddWithValue("d", idDel);
                    int nRows = await comm.ExecuteNonQueryAsync();
                    //Console.Out.WriteLine(String.Format("Number of rows updated={0}", nRows));
                }
                conn.Close();
            }
        }
        internal async Task<List<Unit>> ReadFacilities()
        {
            var list = new List<Unit>();
            using var conn = new NpgsqlConnection(conn_param);
            conn.Open();
            await using (var comm = new NpgsqlCommand($"Select * FROM units", conn))
            {
                var reader = await comm.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    list.Add(new Unit {Id = reader.GetInt32(0), Name = reader.GetString(1), FactoryId = reader.GetInt32(2)});
                }
            }
            conn.Close();
            return list;
        }
        internal async Task DeleteUnit()
        {
            var list = await ReadFacilities();
            list.ForEach(t => t.PrintInfo());
            Console.WriteLine("D_Выберите Id установки для удаления");
            int.TryParse(Console.ReadLine(), out int id);
            using var conn = new NpgsqlConnection(conn_param);
            conn.Open();
            await using (var comm = new NpgsqlCommand($"DELETE FROM units WHERE id = @i", conn))
            {
                comm.Parameters.AddWithValue("i", id);
                int nRows = await comm.ExecuteNonQueryAsync();
                //Console.Out.WriteLine(String.Format("Number of rows deleted={0}", nRows));
            }
            conn.Close();
        }
    }
}
