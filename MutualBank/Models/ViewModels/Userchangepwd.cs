using System.ComponentModel.DataAnnotations.Schema;

namespace MutualBank.Models.ViewModels
{
    public class Userchangepwd
    {

        public string LoginPwd { get; set; }
        public string ConfirmPwd { get; set; }

    }
}
