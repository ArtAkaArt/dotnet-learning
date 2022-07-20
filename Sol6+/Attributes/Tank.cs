namespace Attributes
{
    public class Tank
    {
        [CustomDescription("Циферка")]
        public int Id { get; set; }
        [CustomDescription("Имячко")]
        public string? Name { get; set; }
        [CustomDescription("Буковки")]
        public string? Description { get; set; }
        [AllowedRange(0, 1000), CustomDescription("Сколько занято")]

        public int Volume { get; set; }
        [AllowedRange(200, 1000), CustomDescription("Сколько может быть занято")]
        public int Maxvolume { get; set; }
    }
}