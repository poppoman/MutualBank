using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Area
    {
        public Area()
        {
            Cases = new HashSet<Case>();
        }

        public int AreaId { get; set; }
        public string AreaCity { get; set; } = null!;
        public string AreaTown { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Case> Cases { get; set; }
    }
}
