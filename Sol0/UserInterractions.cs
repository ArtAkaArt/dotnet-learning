using System.Text.Json;
using System.Text;
using Npgsql;

namespace Sol0
{
    public class UserInterractions
    {
        public event EventHandler<CustomEventArgs> UserInput;
        

        internal void SerachUnits(List<Unit> units, List<Tank> tanks, List<Factory> factories, Account acc)
        {
            Console.WriteLine("Введите название резервуара");
            string conn_param = $"Server=127.0.0.1;Port=5432;User Id={acc.AccName};Password={acc.Password};Database=Facilities;";
            var unitName = Console.ReadLine();
            do
            {
                UserInput?.Invoke(this, new CustomEventArgs(DateTime.Now, unitName));
                try
                {
                    var foundUnit = FindUnit(units, tanks, unitName);
                    var factory = FindFactory(factories, foundUnit);

                    Console.WriteLine($"\n{unitName} принадлежит установке {foundUnit.Name} и заводу {factory.Name}\n"); //тоже своего рода Read
                    Console.WriteLine("Для информации о всех установках, Заводах и резервуаров введите R" +
                                    "\nДля создания новой установки введите С" +
                                    "\nДля изменения сведений об установке введите U" +
                                    "\nДля удаления установки введите D");
                    var responce = Console.ReadLine();
                    switch (responce)
                    {
                        case "C":
                            CreateUnit(conn_param);
                            break;
                        case "R":
                            ReadFacility(conn_param);
                            break;
                        case "U":
                            UpdateUnit(conn_param);
                            break;
                        case "D":
                            DeleteUnit(conn_param);
                            break;
                    }
                    Console.WriteLine("Повторите поиск или введите - для завершения");
                    unitName = Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Повторите поиск или введите - для завершения");
                    unitName = Console.ReadLine();
                }
            } while (unitName != "-");
        }

        internal void ShowVolumes(IEnumerable<Tank> tanks)
        {
            Console.WriteLine($"Общая загрузка всех резервуаров {GetTotalVolume(tanks)}");
            Console.WriteLine($"Общая максимальная загрузка всех резервуаров {GetTotalMaxVolume(tanks)}");
        }

        internal int GetTotalVolume(IEnumerable<Tank> tanks)
        {
            return tanks.Sum(t => t.Volume);
        }
        internal int GetTotalMaxVolume(IEnumerable<Tank> tanks)
        {
            return tanks.Sum(t => t.MaxVolume);
        }

        internal Unit FindUnit(IEnumerable<Unit> units, IEnumerable<Tank> tanks, string unitName)
        {
            var tank = tanks.FirstOrDefault(t => t.Name == unitName);
            if (tank is null)
                throw new InvalidOperationException($"Не найдена установка с именем {unitName}");
            var unit = units.FirstOrDefault(t => t.Id == tank.UnitId);
            if (unit is null)
                throw new InvalidOperationException($"Не найдена установка с именем {unitName}");
            return unit;
        }
        internal Factory FindFactory(IEnumerable<Factory> factories, Unit unit)
        {
            //var factory = factories.FirstOrDefault(t => t.Id == unit.FactoryId);
            var factory = (from t in factories
                           where t.Id == unit.FactoryId
                           select t)
                          .FirstOrDefault(); // альтернатива через query, все равно пришлось chain добавить в итоге

            if (factory is null)
                throw new InvalidOperationException($"Не найден завод у установки с именем {unit.Name}");
            return factory;
        }
        //
        void CreateUnit(List<Unit> list)
        {
            var stats = ReadUnitInput();

            if (stats.Length == 3 && Int32.TryParse(stats[0], out int Id) && Int32.TryParse(stats[2], out int FactoryId))
            {
                list.Add(new Unit { Id = Id, Name = stats[1].Trim(), FactoryId = FactoryId });
            }
            SaveToFile(list);
        }
        void CreateUnit(string conn_param)
        {
            string[] input = ReadUnitInput();
            var conn = new NpgsqlConnection(conn_param);
            if (input.Length == 3 && Int32.TryParse(input[0], out int id) && Int32.TryParse(input[2], out int factoryId)) 
            { 
                conn.Open();
                using (var comm = new NpgsqlCommand($"INSERT INTO units (id, name, factoryid) VALUES (@i1, @n1, @f1)", conn))
                {
                    comm.Parameters.AddWithValue("i1", id);
                    comm.Parameters.AddWithValue("n1", input[1].Trim());
                    comm.Parameters.AddWithValue("f1", factoryId);

                    int nRows = comm.ExecuteNonQuery();
                    //Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }
                conn.Close();
            }
        }
        void DeleteUnit(List<Unit> list) 
        {
            list.ForEach(t => t.PrintInfo());
            Console.WriteLine("D_Выберите Id установки для удаления");
            int.TryParse(Console.ReadLine(), out int id);
            var unit = list.FirstOrDefault(t =>t.Id == id);
            if (unit is not null)
                list.Remove(unit);
            SaveToFile(list);
        }
        void DeleteUnit(string conn_param)
        {
            ReadFacility(conn_param);
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
        void UpdateUnit(List<Unit> list)
        {
            list.ForEach(t => t.PrintInfo());
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int id);
            var unit = list.FirstOrDefault(t => t.Id == id);
            var stats = ReadUnitInput();
            if (unit is not null && Int32.TryParse(stats[0], out int Id) && Int32.TryParse(stats[2], out int FactoryId))
            {
                unit.Id = Id;
                unit.Name = stats[1].Trim();
                unit.FactoryId = FactoryId;
            }
            SaveToFile(list);
        }
        void UpdateUnit(string conn_param)
        {
            ReadFacility(conn_param);
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int idDel);
            string[] input = ReadUnitInput();
            if (input.Length == 3 && Int32.TryParse(input[0], out int id) && Int32.TryParse(input[2], out int factoryId))
            {
                var conn = new NpgsqlConnection(conn_param);
                conn.Open();
                using (var comm = new NpgsqlCommand("UPDATE units SET id = @i, name = @n, factoryid = @f WHERE id = @d", conn))
                {

                    comm.Parameters.AddWithValue("i", id);
                    comm.Parameters.AddWithValue("n", input[1].Trim());
                    comm.Parameters.AddWithValue("f", factoryId);
                    comm.Parameters.AddWithValue("d", idDel);
                    int nRows = comm.ExecuteNonQuery();
                    //Console.Out.WriteLine(String.Format("Number of rows updated={0}", nRows));
                }
                conn.Close();
            }
        }

        void ReadFacility(List<Unit> units, List<Tank> tanks, List<Factory> factories)
        {
            Console.WriteLine("R_Введите Unit, Tank или Facility для информации о хранящихся данных");
            var responce = Console.ReadLine();
            switch (responce)
            {
                case "Unit":
                    units.ForEach(t => t.PrintInfo());
                    break;
                case "Tank":
                        tanks.ForEach(t => t.PrintInfo());
                        break;
                case "Facility":
                    factories.ForEach(t => t.PrintInfo());
                    break;
            }
        }
        void ReadFacility(string conn_param)
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

        //блок методов сохранения файлов после CRUD операций
        internal void SaveToFile(List<Unit> list)
        {
            var content = JsonSerializer.Serialize(list);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(),"..\\..\\..\\Jsons\\UnitsTst.json"), content, Encoding.UTF8);
        }
        internal void SaveToFile(List<Factory> list)
        {
            var content = JsonSerializer.Serialize(list);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Jsons\\FactoriesTst.json"), content, Encoding.UTF8);
        }
        internal void SaveToFile(List<Tank> list)
        {
            var content = JsonSerializer.Serialize(list);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\Jsons\\TanksTst.json"), content, Encoding.UTF8);
        }
        internal static string[] ReadUnitInput()
        {
            Console.WriteLine("C_ВВедите через запятую значения для Id, Name и FactoryId");
            var unitStats = Console.ReadLine();
            return unitStats.Split(',');
        }
    }
    public class CustomEventArgs : EventArgs
    {
        public DateTime Date { get; } // перенес дату в аргументы из делегата, для универсальности
        public string Name { get; }
        public CustomEventArgs(DateTime date, string name)
        {
            Date = date;
            Name = name;
        }
    }
}
