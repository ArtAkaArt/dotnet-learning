using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Sol0
{
    class Program
    {
        static internal Account acc = new();
        static string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Jsons");
        public static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false);
            var config = builder.Build();
            var accountInfo = Options.Create<Account>(config.GetSection("Account").Get<Account>());
            acc = accountInfo.Value;
            List<Unit> units = new();
            List<Tank> tanks = new();
            List<Factory> factories = new();
            try
            {
                tanks = GetTanks();
                units = GetUnits();
                factories = GetFactories();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var FRepo = new FacilityRepository(accountInfo.Value); // тоже интересный вопрос где именно экземпляр создавать надо: здесь или в UI
            var user = new UserInterractions(FRepo);
            user.UserInput += (sender, arg) =>
                {
                    Console.WriteLine($"Пользователь ввел название {arg.Name} в {arg.Date.ToString("HH:mm:ss")}"); 
                };
            
            if (tanks is not null && tanks.Any())
                user.ShowVolumes(tanks);
            
            if (tanks is not null && tanks.Any() && units is not null && units.Any() && factories is not null && factories.Any())
                user.SerachUnits(units, tanks, factories);
        }

        public static List<Tank> GetTanks()
        {
            if (FileCheck(Path.Combine(directoryPath, "Tanks.json")))
            {
                var json = File.ReadAllText(Path.Combine(directoryPath, "Tanks.json"));
                return JsonSerializer.Deserialize<List<Tank>>(json);
            }
            throw new Exception("Ошибка загрузки резервуаров");
        }
        public static List<Unit> GetUnits()
        {
            if (FileCheck(Path.Combine(directoryPath, "Units.json.")))
            {
                var json = File.ReadAllText(Path.Combine(directoryPath, "Units.json"));
                return JsonSerializer.Deserialize<List<Unit>>(json);
            }
            throw new Exception("Ошибка загрузки установок.");
        }
        public static List<Factory> GetFactories()
        {
            {
                var json = File.ReadAllText(Path.Combine(directoryPath, "Factories.json"));
                return JsonSerializer.Deserialize<List<Factory>>(json);
            }
            throw new Exception("Ошибка загрузки заводов.");
        }
        static bool FileCheck(string path)// какая-никакая проверка на наличие файлов
        {
            var file = new FileInfo(path);
            return (file.Exists && file.Length > 0);
        }
    }
}
