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
        public List<Skill> GetSkillTags()
        {
            var result = _mutualBankContext.Skills.ToList();
            return result;
        }
        [HttpPost]
        public void AddCase(Case NewCase) {
            //TODO 暫時沒有登入者
            //var UserName = User.Identity.Name;
            //NewCase.CaseUserId = _mutualBankContext.Users.Where(x => x.UserNname == UserName).FirstOrDefault().UserId;

            //整理資料
            NewCase.CaseTitle = NewCase.CaseTitle.Trim();
            NewCase.CaseIntroduction = NewCase.CaseIntroduction.Trim();
            NewCase.CaseAddDate = DateTime.Now;
            //TODO 未有預約發佈功能，暫時設定為立即發佈
            NewCase.CaseReleaseDate = DateTime.Now;
            NewCase.CaseExpireDate = NewCase.CaseReleaseDate.AddDays(14);
            NewCase.CaseClosedDate = DateTime.Now;
            //TODO 圖片未處理
            _mutualBankContext.Cases.Add(NewCase);
            _mutualBankContext.SaveChanges();
        }
        public List<Case> UserCaseModel(bool NeedBit)
        {
            //TODO 暫時沒有登入者
            //var LoginName = User.Identity.Name;
            //var UserId =_mutualBankContext.Logins.FindAsync(LoginName).Result.LoginId;

            //暫時使用11號
            var UserIdTemp = 11;
            var Model = _mutualBankContext.Cases.Where(x => x.CaseUserId == UserIdTemp & x.CaseNeedHelp == NeedBit).OrderBy(x => x.CaseAddDate).ToList();
            return Model;
        }
    }
}
