using System.Text.Json;


namespace Sol0
{
    class Program
    {
        static List<Unit> units = new List<Unit>();
        static List<Tank> tanks = new List<Tank>();
        static List<Factory> factories = new List<Factory>();
        public static void Main()
        {
            var directoryPath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\Jsons\\";
          
            if (FileCheck(directoryPath + "Units.json"))
            {
                var unitsJson = File.ReadAllText(directoryPath + "Units.json");
                units = JsonSerializer.Deserialize<List<Unit>>(unitsJson);
            }
            if (FileCheck(directoryPath + "Tanks.json"))
            {
                var unitsJson = File.ReadAllText(directoryPath + "Tanks.json");
                tanks = JsonSerializer.Deserialize<List<Tank>>(unitsJson);
            }
            if (FileCheck(directoryPath + "Factories.json"))
            {
                var unitsJson = File.ReadAllText(directoryPath + "Factories.json");
                factories = JsonSerializer.Deserialize<List<Factory>>(unitsJson);
            }
            if (tanks != null && tanks.Any())
            {
                tanks.ForEach(t => GetTankInfo(t));
                Console.WriteLine($"Общая загрузка всех резервуаров {tanks.Sum(t => t.Volume)}");
                Console.WriteLine($"Общая максимальная загрузка всех резервуаров {tanks.Sum(t => t.MaxVolume)}");
            }
        }
        static bool FileCheck(string path2)// какая-никакая проверка на наличие файлов
        {
            var file2 = new FileInfo(path2);
            return (file2.Exists && file2.Length > 0);
        }
        static void GetTankInfo(Tank tank)
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
            Console.WriteLine($"Названия Резервуара: {tank.Name}, название установки: {unitName}, название завода: {factoryName}");
        }
    }
}
