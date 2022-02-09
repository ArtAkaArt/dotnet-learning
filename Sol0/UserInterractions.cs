namespace Sol0
{
    public class UserInterractions
    {
        public delegate void UserInputHandler(string msg);
        public event UserInputHandler Notify;

        internal void SerachUnits(IEnumerable<Unit> units, IEnumerable<Tank> tanks, IEnumerable<Factory> factories)
        {
            Console.WriteLine("Введите название резервуара");
            var unitName = Console.ReadLine();
            do
            {
                Notify?.Invoke(unitName);
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
    }
}
