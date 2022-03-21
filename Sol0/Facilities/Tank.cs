namespace Sol0
{
    public class Tank: IFacility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
        public int UnitId { get; set; }
        public void PrintInfo() => Console.WriteLine($"Id = {Id}, Name = {Name}, Volume = {Volume}, MaxVolume = {MaxVolume}, UnitId = {UnitId}");
    }
}
