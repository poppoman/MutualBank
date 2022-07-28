using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        private static string _filePath = Path.Combine("/Img", "CasePhoto");

        public ProfileController(MutualBankContext mutualBankContext)
        { 
            _mutualBankContext = mutualBankContext;
        }
        
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Model = _mutualBankContext.Users.Include("UserNavigation").Include("UserSkill").Where(x => x.UserId == id)
                .Select(x => new {
                    UserNname = x.UserNname,
                    UserHphoto = x.UserHphoto,//TODO 之後要改為圖檔路徑
                    UserSkillName = x.UserSkill.SkillName,
                    UserAreaName = $"{x.UserNavigation.AreaCity}{x.UserNavigation.AreaTown}",
                    UserCv = x.UserCv,
                    UserResume = x.UserResume,
                    UserSchool = x.UserSchool,
                    UserFaculty = x.UserFaculty,
                    UserPoint = x.UserPoint
                }).First();

            //ViewBag帶該User的Case
            var CaseModel = _mutualBankContext.Cases.Include("CaseSkil").Where(x => x.CaseUserId == id & x.CaseClosedDate >= DateTime.Now)
               .Select(x => new CaseViewModel
               {
                   CaseId = x.CaseId,
                   CaseNeedHelp = x.CaseNeedHelp,
                   CaseReleaseDate = x.CaseReleaseDate,
                   CaseExpireDate = x.CaseExpireDate,
                   CaseTitle = x.CaseTitle,
                   CaseIntroduction = x.CaseIntroduction,
                   CasePhoto = Path.Combine(_filePath, x.CasePhoto),
                   CaseSerDate = x.CaseSerDate,
                   CaseSerArea = x.CaseSerArea,
                   CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                   CaseSkillId = x.CaseSkil.SkillId,
                   CaseSkillName = x.CaseSkil.SkillName,
                   CaseUserId = x.CaseUser.UserId,
                   CaseUserName = x.CaseUser.UserNname,
                   MessageCount = x.Messages.Count
               });
            ViewBag.Case = Newtonsoft.Json.JsonConvert.SerializeObject(CaseModel);
            return View(Model);
        }

    }
}
