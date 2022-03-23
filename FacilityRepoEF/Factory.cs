using System.ComponentModel.DataAnnotations;

namespace FacilityRepoEF
{
    public class Factory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
