using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Extensions;
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
        
        [HttpGet]
        public string GetUserCaseModel()
        {
            var UserId = this.User.GetId();
            var PhotoFileFolder = Path.Combine("/Img", "CasePhoto");
            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("Messages")
                .Where(x => x.CaseUserId == UserId).Select(x => new CaseViewModel
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    IsCaseExpire=DateTime.Now >= x.CaseExpireDate?true:false,
                    CaseTitle = x.CaseTitle,
                    CaseIntroduction = x.CaseIntroduction,
                    CasePhoto = Path.Combine(PhotoFileFolder, x.CasePhoto),
                    CaseSerDate = x.CaseSerDate,
                    CaseSerArea = x.CaseSerArea,
                    CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                    CaseSkillId = x.CaseSkil.SkillId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CaseUserId = x.CaseUser.UserId,
                    CaseUserName = $"{x.CaseUser.UserLname}{x.CaseUser.UserFname}",
                    MessageCount = x.Messages.Count
                });


            var ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            return ModelJson;
        }

        [HttpPost]
        public void AddCase(Case NewCase)
        {

            //整理資料
            NewCase.CaseUserId = this.User.GetId();
            NewCase.CaseTitle = NewCase.CaseTitle.Trim();
            NewCase.CaseIntroduction = NewCase.CaseIntroduction.Trim();
            NewCase.CaseAddDate = DateTime.Now;
            NewCase.CaseExpireDate = NewCase.CaseReleaseDate.AddDays(14);
            NewCase.CaseClosedDate = DateTime.Now;

            //取出表單圖片及圖片檔名
            IFormFile InputFile = null;
            if (HttpContext.Request.Form.Files.Count == 0)
            {
                NewCase.CasePhoto = "0_Default.jpg";
            }
            else
            {
                //制定圖片檔名
                InputFile = HttpContext.Request.Form.Files[0];
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                NewCase.CasePhoto = $"{NewCase.CaseUserId}_{UniqueId}.{PhotoFormat}";
                //存圖
                var InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img", "CasePhoto", NewCase.CasePhoto);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs);
                fs.Close();
            }


            _mutualBankContext.Cases.Add(NewCase);
            _mutualBankContext.SaveChanges();
        }
    }
}
