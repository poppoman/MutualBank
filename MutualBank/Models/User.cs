using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class User
    {
        public User()
        {
            Cases = new HashSet<Case>();
        }
        public int UserId { get; set; }
        public string UserLname { get; set; } = null!;
        public string UserFname { get; set; } = null!;
        public string UserNname { get; set; } = null!;
        public bool UserSex { get; set; }
        public string? UserHphoto { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTime? UserBirthday { get; set; }
        public int UserSkillId { get; set; }
        public int UserAreaId { get; set; }
        public string? UserCv { get; set; }
        public string? UserResume { get; set; }
        public string? UserSchool { get; set; }
        public string? UserFaculty { get; set; }
        public int UserPoint { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
    }
}
