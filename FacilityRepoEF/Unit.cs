using System.ComponentModel.DataAnnotations;

namespace FacilityRepoEF
{
    public class Unit
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int FactoryId { get; set; }

    }
}

