using Microsoft.AspNetCore.Mvc;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class CaseController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        public CaseController(MutualBankContext MutualBankContext)
        {
            _mutualBankContext = MutualBankContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetPostCase() 
        {
            return PartialView("PostCase");
        }
        public IActionResult GetCaseList()
        {
            return PartialView("CaseList");
        }
        public List<Case> UserCaseModel(bool NeedBit)
        {
            //目前登入者(未有)
            //var LoginName = User.Identity.Name;
            //var UserId =_mutualBankContext.Logins.FindAsync(LoginName).Result.LoginId;
            
            //暫時的UserId
            var UserIdTemp = 11;
            var Model = _mutualBankContext.Cases.Where(x => x.CaseUserId == UserIdTemp & x.CaseNeedHelp == NeedBit).OrderBy(x => x.CaseAddDate).ToList();
            return Model;
        }
    }
}
