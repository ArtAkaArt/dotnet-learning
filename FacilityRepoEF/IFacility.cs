namespace FacilityContextLib
{
    public interface IFacility
    {
        public abstract int Id { get; }
        public abstract string? Name { get; }
        public abstract string? Description { get; }
    }
}
