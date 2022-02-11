using System.Text.Json;
using System.Text;

namespace Sol0
{
    public class UserInterractions
    {
        public event EventHandler<CustomEventArgs> Notify; //нашел как предустановленный делегат использовать с кастомным arg

        internal void SerachUnits(List<Unit> units, List<Tank> tanks, List<Factory> factories)
        {
            Console.WriteLine("Введите название резервуара");
            var unitName = Console.ReadLine();
            do
            {
                Notify?.Invoke(this, new CustomEventArgs(DateTime.Now, unitName));
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
                            CreateUnit(units);
                            break;
                        case "R":
                            ReadFacility(units, tanks, factories);
                            break;
                        case "U":
                            UpdateUnit(units);
                            break;
                        case "D":
                            DeleteUnit(units);
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
            Console.WriteLine("C_ВВедите через запятую значения для Id, Name и FactoryId");
            var unitStats = Console.ReadLine();
            var stats = unitStats.Split(',');

            if (stats.Length == 3 && Int32.TryParse(stats[0], out int stat1) && Int32.TryParse(stats[2], out int stat3))
            {
                list.Add(new Unit { Id = stat1, Name = stats[1], FactoryId = stat3 });
            }
            SaveToFile(list);
        }
        void DeleteUnit(List<Unit> list) 
        {
            list.ForEach(t => Console.WriteLine($"Id = {t.Id}, Name = {t.Name}, FactoryId = {t.FactoryId}"));
            Console.WriteLine("D_Выберите Id установки для удаления");
            int.TryParse(Console.ReadLine(), out int id);
            var unit = list.FirstOrDefault(t =>t.Id == id);
            if (unit is not null)
                list.Remove(unit);
            SaveToFile(list);
        }
        void UpdateUnit(List<Unit> list)
        {
            list.ForEach(t => Console.WriteLine($"Id = {t.Id}, Name = {t.Name}, FactoryId = {t.FactoryId}"));
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int id);
            var unit = list.FirstOrDefault(t => t.Id == id);
            Console.WriteLine("C_ВВедите через запятую значения для Id, Name и FactoryId");
            var unitStats = Console.ReadLine();
            var stats = unitStats.Split(',');
            if (unit is not null && Int32.TryParse(stats[0], out int stat1) && Int32.TryParse(stats[2], out int stat3))
            {
                unit.Id = stat1;
                unit.Name = stats[1];
                unit.FactoryId = stat3;
            }
            SaveToFile(list);
        }
        void ReadFacility(List<Unit> units, List<Tank> tanks, List<Factory> factories)
        {
            Console.WriteLine("R_Введите Unit, Tank или Facility для информации о хранящихся данных");
            var responce = Console.ReadLine();
            switch (responce)
            {
                case "Unit":
                    units.ForEach(t => Console.WriteLine($"Id = {t.Id}, Name = {t.Name}, FactoryId = {t.FactoryId}"));
                    break;
                case "Tank":
                        tanks.ForEach(t => Console.WriteLine($"Id = {t.Id}, Name = {t.Name}, UnitId = {t.UnitId}, Volume = {t.Volume}, MaxVolume = {t.MaxVolume}"));
                        break;
                case "Facility":
                    factories.ForEach(t => Console.WriteLine($"Id = {t.Id}, Name = {t.Name}, Description = {t.Description}"));
                    break;
            }
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
