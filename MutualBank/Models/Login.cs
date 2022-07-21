using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string LoginName { get; set; } = null!;
        public string LoginPwd { get; set; } = null!;
        public string LoginEmail { get; set; } = null!;
        public bool LoginLevel { get; set; }
        public DateTime LoginAddDate { get; set; }
        public bool LoginActive { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
