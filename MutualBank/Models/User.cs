using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class User
    {
        public User()
        {
            Cases = new HashSet<Case>();
            Messages = new HashSet<Message>();
        }

        public int UserId { get; set; }
        public string? UserLname { get; set; }
        public string? UserFname { get; set; }
        public string? UserNname { get; set; }
        public bool? UserSex { get; set; }
        public string? UserHphoto { get; set; }
        public string? UserEmail { get; set; }
        public DateTime? UserBirthday { get; set; }
        public int? UserSkillId { get; set; }
        public int? UserAreaId { get; set; }
        public string? UserCv { get; set; }
        public string? UserResume { get; set; }
        public string? UserSchool { get; set; }
        public string? UserFaculty { get; set; }
        public int UserPoint { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
