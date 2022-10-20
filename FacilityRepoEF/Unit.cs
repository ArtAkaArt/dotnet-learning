namespace FacilityContextLib
{
    public class Unit : IFacility
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int FactoryId { get; set; }
        public Factory? Factory { get; set; }
    }
}
