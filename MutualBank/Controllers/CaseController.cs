using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class CaseController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CaseController(MutualBankContext MutualBankContext,IWebHostEnvironment webHostEnvironment)
        {
            _mutualBankContext = MutualBankContext;
            _webHostEnvironment = webHostEnvironment;
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
            //NewCase.CaseUserId = _mutualBankContext.Users.Where(x => Model.UserNname == UserName).FirstOrDefault().UserId;
            
            //整理資料
            //文字
            NewCase.CaseTitle = NewCase.CaseTitle.Trim();
            NewCase.CaseIntroduction = NewCase.CaseIntroduction.Trim();
            //日期
            NewCase.CaseAddDate = DateTime.Now;
            NewCase.CaseExpireDate = NewCase.CaseReleaseDate.AddDays(14);
            NewCase.CaseClosedDate = DateTime.Now;

            //取出表單圖片及名稱 
            var InpitFile = HttpContext.Request.Form.Files[0];
            NewCase.CasePhoto = InpitFile.FileName;
            //TODO 檔案暫時儲存在wwwroot
            var InpitFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img","CasePhoto", NewCase.CasePhoto);
            FileStream fs = new FileStream(InpitFilePath, FileMode.Create);
            fs.CopyToAsync(fs);
            fs.Close();

            _mutualBankContext.Cases.Add(NewCase);
            _mutualBankContext.SaveChanges();
        }
        [HttpGet]
        public string GetUserCaseModel()
        {
            //目前user 暫時沒有，暫時使用11號
            //var LoginName = User.Identity.Name;
            //var UserId =_mutualBankContext.Logins.FindAsync(LoginName).Result.LoginId;

            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("Messages")
                .Where(x => x.CaseUserId == 11).Select(x => new CaseViewModel 
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    CaseTitle = x.CaseTitle,
                    CaseIntroduction = x.CaseIntroduction,
                    CasePhoto = x.CasePhoto,
                    CaseSerDate = x.CaseSerDate,
                    CaseSerArea = x.CaseSerArea,
                    CaseSkillId = x.CaseSkil.SkillId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CaseUserId = x.CaseUser.UserId,
                    CaseUserName = x.CaseUser.UserFname.Concat(x.CaseUser.UserLname).ToString(),
                    MessageCount = x.Messages == null ? 0 : x.Messages.Count()
                });


            var ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            return ModelJson;
        }
    }
}
