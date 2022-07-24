using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class CaseController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CaseController(MutualBankContext MutualBankContext, IWebHostEnvironment webHostEnvironment)
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
        public void AddCase(Case NewCase)
        {
            //TODO 登入者Claims
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "xxx").Value);
            Console.WriteLine("========目前使用者 id========");
            Console.WriteLine(UserId);


            //整理資料
            NewCase.CaseUserId = UserId;
            NewCase.CaseTitle = NewCase.CaseTitle.Trim();
            NewCase.CaseIntroduction = NewCase.CaseIntroduction.Trim();
            NewCase.CaseAddDate = DateTime.Now;
            NewCase.CaseExpireDate = NewCase.CaseReleaseDate.AddDays(14);
            NewCase.CaseClosedDate = DateTime.Now;

            //取出表單圖片及名稱 
            var InputFile = HttpContext.Request.Form.Files[0];
            var InputFilePath = "";
            if (InputFile == null)
            {
                NewCase.CasePhoto = "0_Default.jpg";
            }
            else 
            {
                //儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                NewCase.CasePhoto = $"{UserId}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img", "CasePhoto", NewCase.CasePhoto);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs);
                fs.Close();
            }
            

            _mutualBankContext.Cases.Add(NewCase);
            _mutualBankContext.SaveChanges();
        }
        [HttpGet]
        public string GetUserCaseModel()
        {
            //TODO 登入者Claims
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "xxx").Value);

            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("Messages")
                .Where(x => x.CaseUserId == UserId).Select(x => new CaseViewModel
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
                    CaseUserName = $"{x.CaseUser.UserLname}{x.CaseUser.UserFname}",
                    MessageCount = x.Messages.Count
                });


            var ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            return ModelJson;
        }
    }
}
