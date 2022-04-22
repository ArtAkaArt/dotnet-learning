namespace Sol0
{
    public class Unit : IFacility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FactoryId { get; set; }
        public void PrintInfo() => Console.WriteLine($"Id = {Id}, Name = {Name}, FactoryId = {FactoryId}");
    }
}
