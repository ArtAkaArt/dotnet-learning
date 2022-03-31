namespace FacilityContextLib
{
    public class Factory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IList<Unit>? Units { get; set; }
    }
}
