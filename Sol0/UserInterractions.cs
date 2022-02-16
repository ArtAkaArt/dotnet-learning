using System.Text.Json;
using System.Text;

namespace Sol0
{
    public class UserInterractions
    {
        public event EventHandler<CustomEventArgs> UserInput;

        internal void SerachUnits(List<Unit> units, List<Tank> tanks, List<Factory> factories)
        {
            Console.WriteLine("Введите название резервуара");
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
                            DBRequester.CreateUnit();
                            break;
                        case "R":
                            DBRequester.ReadFacility();
                            break;
                        case "U":
                            DBRequester.UpdateUnit();
                            break;
                        case "D":
                            DBRequester.DeleteUnit();
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
        
        void CreateUnit(List<Unit> list)
        {
            if (ReadUnitInput(out InputContainer stats))
            {
                list.Add(new Unit { Id = stats.Id, Name = stats.Name, FactoryId = stats.FactoryId });
            }
            SaveToFile(list);
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
        
        void UpdateUnit(List<Unit> list)
        {
            list.ForEach(t => t.PrintInfo());
            Console.WriteLine("U_Выберите Id установки для изменения");
            int.TryParse(Console.ReadLine(), out int id);
            var unit = list.FirstOrDefault(t => t.Id == id);
            if (ReadUnitInput(out InputContainer stats))
            {
                unit.Id = stats.Id;
                unit.Name = stats.Name;
                unit.FactoryId = stats.FactoryId;
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

        //блок методов сохранения файлов после CRUD операций
        internal void SaveToFile(List<Unit> list)
        {
            var content = JsonSerializer.Serialize(list);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(),"..\\..\\..\\Jsons\\UnitsTst.json"), content, Encoding.UTF8);
        }
        internal static bool ReadUnitInput(out InputContainer container) // зачем-то кортежи придумали же
        {
            Console.WriteLine("C_ВВедите через запятую значения для Id, Name и FactoryId");
            var unitLine = Console.ReadLine();
            var input= unitLine.Split(',');
            if (input.Length == 3 && Int32.TryParse(input[0], out int id) && Int32.TryParse(input[2], out int factoryId))
            {
                container = new (id,input[1].Trim(), factoryId);
                return true;
            }
            container = new InputContainer(0, null,0) ;
            return false;
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
