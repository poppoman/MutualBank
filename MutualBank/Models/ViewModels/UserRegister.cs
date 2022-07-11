using System.ComponentModel.DataAnnotations.Schema;

namespace MutualBank.Models.ViewModels
{
    public class UserRegister
    {
        public string LoginName { get; set; }

        public string LoginPwd { get; set; }

        public string LoginEmail { get; set; }

        public string ConfirmPwd { get; set; }
    }
}
