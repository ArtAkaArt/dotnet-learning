using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityContextLib
{
    public class Unit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int FactoryId { get; set; }
        public Factory? Factory { get; set; }
    }
}
