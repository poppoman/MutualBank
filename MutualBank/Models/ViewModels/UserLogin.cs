using System.ComponentModel.DataAnnotations.Schema;

namespace MutualBank.Models.ViewModels
{
    public class UserLogin
    {
        public string LoginName { get; set; }

        public string LoginPwd { get; set; }

    }
}
