using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityContextLib
{
    public class Factory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IList<Unit> Units { get; set; }
    }
}
