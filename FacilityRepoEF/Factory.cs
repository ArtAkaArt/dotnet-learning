using System;
using System.Collections.Generic;

namespace FacilityRepoEF
{
    public partial class Factory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
