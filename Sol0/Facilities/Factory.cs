namespace Sol0
{
    internal class Factory : IFacility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public void PrintInfo() => Console.WriteLine($"Id = {Id}, Name = {Name}, Description = {Description}");
    }
}
