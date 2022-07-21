using Attributes;
namespace FacilityContextLib
{
    public class Tank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [AllowedRange(0, 1000), CustomDescription("Сколько занято")]
        public int Volume { get; set; }
        [AllowedRange(200, 1000), CustomDescription("Сколько может быть занято")]
        public int Maxvolume { get; set; }
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }
    }
}
