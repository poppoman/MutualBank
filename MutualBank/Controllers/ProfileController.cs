using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        private static string _casePhotoFilePath = Path.Combine("/Img", "CasePhoto");
        private static string _userPhotoFilePath = Path.Combine("/Img", "User");
        private int ExpireDays = 120;

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
                    UserHphoto = x.UserNavigation.User.UserHphoto == null ? x.UserNavigation.User.UserSex == true ? Path.Combine(_userPhotoFilePath, "Male.PNG") : Path.Combine(_userPhotoFilePath, "Female.PNG") : Path.Combine(_userPhotoFilePath, x.UserNavigation.User.UserHphoto),
                    UserSkillName = x.UserSkill.SkillName,
                    UserAreaName = $"{x.UserNavigation.AreaCity}{x.UserNavigation.AreaTown}",
                    UserCv = x.UserCv,
                    UserResume = x.UserResume,
                    UserSchool = x.UserSchool,
                    UserFaculty = x.UserFaculty,
                    UserPoint = x.UserPoint
                }).First();

            //ViewBag帶該User的Case
            var CaseModel = _mutualBankContext.Cases.Include("CaseSkil")
                .Where(x => x.CaseUserId == id &  x.CaseReleaseDate <= DateTime.Now && x.CaseExpireDate <= DateTime.Now.AddDays(ExpireDays) && x.CaseIsExecute == false)
               .Select(x => new CaseViewModel
               {
                   CaseId = x.CaseId,
                   CaseNeedHelp = x.CaseNeedHelp,
                   CaseReleaseDate = x.CaseReleaseDate,
                   CaseExpireDate = x.CaseExpireDate,
                   CaseTitle = x.CaseTitle,
                   CaseIntroduction = x.CaseIntroduction,
                   CasePhoto = Path.Combine(_casePhotoFilePath, x.CasePhoto),
                   CaseSerDate = x.CaseSerDate,
                   CaseSerArea = x.CaseSerArea,
                   CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                   CaseSkillId = x.CaseSkil.SkillId,
                   CaseSkillName = x.CaseSkil.SkillName,
                   CaseUserId = x.CaseUser.UserId,
                   CaseUserName = x.CaseUser.UserNname,
                   MessageCount = x.Messages.Count,
                   UserPhoto = x.CaseUser.UserHphoto == null ? x.CaseUser.UserSex == true ? Path.Combine(_userPhotoFilePath, "Male.PNG") : Path.Combine(_userPhotoFilePath, "Female.PNG") : Path.Combine(_userPhotoFilePath, x.CaseUser.UserHphoto)
               });
            ViewBag.Case = Newtonsoft.Json.JsonConvert.SerializeObject(CaseModel);
            return View(Model);
        }


        public IActionResult GetProfileSetting() 
        {
            return PartialView("ProfileSetting");
        }

    }
}
