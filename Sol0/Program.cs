using System.Text.Json;

namespace Sol0
{
    class Program
    {
        static List<Unit> units = new();
        static List<Tank> tanks = new();
        static List<Factory> factories = new();
        public static void Main()
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Jsons\\");

            if (FileCheck(Path.Combine(directoryPath, "Units.json")))
            {
                var json = File.ReadAllText(Path.Combine(directoryPath, "Units.json"));
                units = JsonSerializer.Deserialize<List<Unit>>(json);
            }
            if (FileCheck(directoryPath + "Tanks.json"))
            {
                var json = File.ReadAllText(Path.Combine(directoryPath, "Tanks.json"));
                tanks = JsonSerializer.Deserialize<List<Tank>>(json);
            }
            if (FileCheck(directoryPath + "Factories.json"))
            {
                var json = File.ReadAllText(directoryPath + "Factories.json");
                factories = JsonSerializer.Deserialize<List<Factory>>(json);
            }
            if (tanks != null && tanks.Any())
            {
                //tanks.ForEach(t => GetTankInfo(t));
                Console.WriteLine($"Общая загрузка всех резервуаров {GetTotalVolume(tanks)}");
                Console.WriteLine($"Общая максимальная загрузка всех резервуаров {GetTotalMaxVolume(tanks)}");
            }
            Console.WriteLine("Введите название резервуара");
            var unitName = Console.ReadLine();
            do
            {
                try 
                { 
                    var foundUnit = FindUnit(units, tanks, unitName);
                    var factory = FindFactory(factories, foundUnit);
                
                    Console.WriteLine($"{unitName} принадлежит установке {foundUnit.Name} и заводу {factory.Name}");
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
        
        public static int GetTotalVolume(IEnumerable<Tank> tanks)
        {
            return tanks.Sum(t => t.Volume);
        }
        public static int GetTotalMaxVolume(IEnumerable<Tank> tanks)
        {
            return tanks.Sum(t => t.MaxVolume);
        }

        public static Unit FindUnit(IEnumerable<Unit> units, IEnumerable<Tank> tanks, string unitName)
        {
            var tank = tanks.Where(t => t.Name == unitName)
                            .FirstOrDefault();
            if (tank is null) 
                throw new InvalidOperationException($"Не найдена установка с именем {unitName}");
            var unit = units.Where(t => t.Id == tank.UnitId)
                            .FirstOrDefault();
            if (unit is null) 
                throw new InvalidOperationException($"Не найдена установка с именем {unitName}");
            return unit;
        }

        public static Factory FindFactory(IEnumerable<Factory> factories, Unit unit)
        {
            var factory = factories.Where(t => t.Id == unit.FactoryId)
                            .FirstOrDefault();
            if (factory is null)
                throw new InvalidOperationException($"Не найден завод у установки с именем {unit.Name}");
            return factory;
        }
        static bool FileCheck(string path)// какая-никакая проверка на наличие файлов
        {
            var file = new FileInfo(path);
            return (file.Exists && file.Length > 0);
        }
        static void GetTankInfo(Tank tank)
        {
            try
            {
                var unitName = units.Where(t => t.Id == tank.UnitId)
                                        .Select(t => t.Name)
                                        .First();

                var factoryId = units.Where(t => t.Id == tank.UnitId)
                                        .Select(t => t.FactoryId)
                                        .First();

                var factoryName = factories.Where(t => t.Id == factoryId)
                                            .Select(t => t.Name)
                                            .First();
                Console.WriteLine($"{tank.Name} принадлежит установке {unitName} и заводу {factoryName}");
            }
            catch (InvalidOperationException ex)//свою ошибку генерировать не стал, вроде бы незачем. Тут еще не нравится, что эта ошибка может возникнуть в трех местах
            {
                Console.WriteLine(ex + "\n\tОшибка: вероятно не найден первый элемент последовательности.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
