using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Skill
    {
        public Skill()
        {
            Cases = new HashSet<Case>();
            Users = new HashSet<User>();
        }

        public int SkillId { get; set; }
        public string SkillName { get; set; } = null!;

        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
