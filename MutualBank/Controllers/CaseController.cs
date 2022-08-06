using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Extensions;
using MutualBank.Models;
using MutualBank.Models.ReturnMsg;

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
            ViewBag.UserPoint = _mutualBankContext.Users.First(x => x.UserId == User.GetId()).UserPoint;
            return PartialView("PostCase");
        }
        public IActionResult GetCaseList()
        {
            return PartialView("CaseList");
        }

        public IActionResult GetExecuteCase()
        {
            return PartialView("ExecuteCase");
        }
        public List<Skill> GetSkillTags()
        {
            var result = _mutualBankContext.Skills.ToList();
            return result;
        }

        [HttpGet]
        public string GetUserCaseModel()
        {
            var UserId = User.GetId();
            var PhotoFileFolder = Path.Combine("/Img", "CasePhoto");
            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("Messages")
                .Where(x => x.CaseUserId == UserId).Select(x => new CaseViewModel
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    IsCaseExpire = DateTime.Now >= x.CaseExpireDate ? true : false,
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
                }).OrderByDescending(x=>x.CaseReleaseDate).ToList();

            var ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            return ModelJson;
        }

        [HttpPost]
        public void AddCase(Case NewCase)
        {
            NewCase.CaseUserId = User.GetId();
            NewCase.CaseTitle = NewCase.CaseTitle.Trim();
            NewCase.CaseIntroduction = NewCase.CaseIntroduction.Trim();
            NewCase.CaseAddDate = DateTime.Now;
            NewCase.CaseExpireDate = NewCase.CaseReleaseDate.AddDays(14);
            NewCase.CaseIsExecute = false;
            
            if (HttpContext.Request.Form.Files.Count == 0)
            {
                NewCase.CasePhoto = "0_Default.jpg";
            }
            else
            {
                IFormFile InputFile = HttpContext.Request.Form.Files[0];
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                NewCase.CasePhoto = $"{NewCase.CaseUserId}_{UniqueId}.{PhotoFormat}";
                var InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img", "CasePhoto", NewCase.CasePhoto);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyToAsync(fs);
                fs.Close();
            }
            _mutualBankContext.Cases.Add(NewCase);
            _mutualBankContext.SaveChanges();
        }

        public JsonResult GetExecuteCaseModel()
        {
            var id = User.GetId();
            var CaseModel = _mutualBankContext.Cases.Include("CaseUser").Include("Points").Include("CaseSkil")
                .Where(x => x.CaseUserId == id & x.CaseIsExecute == true & x.CaseClosedDate == null)
                .Select(x => new 
                {
                    CaseId=x.CaseId,
                    CaseTitle=x.CaseTitle,
                    CaseUserId=x.CaseUserId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CasePoint=x.CasePoint,
                    IsNeed = x.CaseNeedHelp,
                    TargetUserId = x.Points.Where(y => y.PointNeedHelp != x.CaseNeedHelp)
                    .Select(y => y.PointUserId).First(),
                    TransDate = x.Points.Where(y => y.PointUserId == x.CaseUserId).Select(y => y.PointAddDate).First(),
                    TargetUserName= _mutualBankContext.Users.Where(y=>y.UserId== x.Points.Where(y => y.PointNeedHelp != x.CaseNeedHelp)
                    .Select(y => y.PointUserId).First()).First().UserNname
                }).OrderBy(x=>x.TransDate).ToList();
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(CaseModel));
        }

        public IActionResult CaseDone(int CaseId)
        {
            //update case
            var Case = _mutualBankContext.Cases.Where(x => x.CaseId == CaseId).First();
            Case.CaseClosedDate = DateTime.Now;
            var PointLog = _mutualBankContext.Points.Where(x => x.PointUserId == Case.CaseUserId & x.PointCaseId == CaseId).First();
            var PointTragetLog = _mutualBankContext.Points.Where(x => x.PointUserId != Case.CaseUserId & x.PointCaseId == CaseId).First();
            PointLog.PointIsDone = PointTragetLog.PointIsDone = true;
            //trans & update point
            var User = _mutualBankContext.Users.Where(x => x.UserId == PointLog.PointUserId).First();
            var TargetUser = _mutualBankContext.Users.Where(x => x.UserId == PointTragetLog.PointUserId).First();

            var TransPoint = Case.CasePoint;
            if (Case.CaseNeedHelp)
            {
                User.UserPoint -= TransPoint;
                TargetUser.UserPoint += TransPoint;
            }
            else
            {
                User.UserPoint += TransPoint;
                TargetUser.UserPoint -= TransPoint;
            }

            var Msg = new ReturnMsg();
            try
            {
                _mutualBankContext.SaveChanges();
                Msg.code = 200;
                Msg.Msg = Convert.ToString(_mutualBankContext.Users.Where(x => x.UserId == this.User.GetId()).Select(x => x.UserPoint).FirstOrDefault());
            }
            catch (DbUpdateException ex)
            {
                Msg.code = 400;
                Msg.Msg = "Error";
                throw;
            }
            return StatusCode(Msg.code, Msg);
        }
    }
}
