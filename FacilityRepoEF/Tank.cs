using System.ComponentModel.DataAnnotations;

namespace FacilityRepoEF
{
    public class Tank
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Volume { get; set; }
        [Required]
        public int MaxVolume { get; set; }
        [Required]
        public int UnitId { get; set; }
    }
}
