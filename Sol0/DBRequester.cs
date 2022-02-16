using Npgsql;
using static Sol0.UserInterractions;
using static Sol0.Program;

namespace Sol0
{
    internal class DBRequester
    {
        static string conn_param;
        static DBRequester()
        {
            conn_param = $"Server=127.0.0.1;Port=5432;User Id={acc.AccName};Password={acc.Password};Database=Facilities;";
        }
        internal static void CreateUnit()//string conn_param
        {
            var conn = new NpgsqlConnection(conn_param);
            if (ReadUnitInput(out InputContainer stats))
            {
                conn.Open();
                using (var comm = new NpgsqlCommand($"INSERT INTO units (id, name, factoryid) VALUES (@i1, @n1, @f1)", conn))
                {
                    comm.Parameters.AddWithValue("i1", stats.Id);
                    comm.Parameters.AddWithValue("n1", stats.Name);
                    comm.Parameters.AddWithValue("f1", stats.FactoryId);

                    int nRows = comm.ExecuteNonQuery();
                    //Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }
                conn.Close();
            }
        }
        internal static void UpdateUnit()
        {
            ReadFacility();
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int idDel);
            if (ReadUnitInput(out InputContainer stats))
            {
                var conn = new NpgsqlConnection(conn_param);
                conn.Open();
                using (var comm = new NpgsqlCommand("UPDATE units SET id = @i, name = @n, factoryid = @f WHERE id = @d", conn))
                {

                    comm.Parameters.AddWithValue("i", stats.Id);
                    comm.Parameters.AddWithValue("n", stats.Name);
                    comm.Parameters.AddWithValue("f", stats.FactoryId);
                    comm.Parameters.AddWithValue("d", idDel);
                    int nRows = comm.ExecuteNonQuery();
                    //Console.Out.WriteLine(String.Format("Number of rows updated={0}", nRows));
                }
                conn.Close();
            }
        }
        internal static void ReadFacility()
        {
            var conn = new NpgsqlConnection(conn_param);
            conn.Open();
            using (var comm = new NpgsqlCommand($"Select * FROM units", conn))
            {
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Id = {reader.GetInt32(0)}, Name = {reader.GetString(1)}, FactoryId = {reader.GetInt32(2)}");
                }
            }
            conn.Close();
        }
        internal static void DeleteUnit()
        {
            ReadFacility();
            Console.WriteLine("D_Выберите Id установки для удаления");
            int.TryParse(Console.ReadLine(), out int id);
            var conn = new NpgsqlConnection(conn_param);
            conn.Open();
            using (var comm = new NpgsqlCommand($"DELETE FROM units WHERE id = @i", conn))
            {
                comm.Parameters.AddWithValue("i", id);
                int nRows = comm.ExecuteNonQuery();
                //Console.Out.WriteLine(String.Format("Number of rows deleted={0}", nRows));
            }
            conn.Close();
        }
    }
}
