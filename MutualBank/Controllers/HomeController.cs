using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;
using System.Diagnostics;

namespace MutualBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MutualBankContext _mutualBankContext;
        private static string _casePhotoFilePath= Path.Combine("/Img", "CasePhoto");
        private static string _userPhotoFilePath = Path.Combine("/Img", "User");
        //過期日的篩選天數(為撈到更多卡片，暫時設定為120天)
        private int ExpireDays = 120;
        public HomeController(ILogger<HomeController> logger, MutualBankContext mutualBankContext)
        {
            _logger = logger;
            _mutualBankContext = mutualBankContext;
        }

        public IActionResult Index()
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            Console.WriteLine(DateTime.Now.AddDays(ExpireDays));
            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("CaseUser")
                .Where(x => x.CaseReleaseDate <= DateTime.Now && x.CaseExpireDate <= DateTime.Now.AddDays(ExpireDays) && x.CaseIsExecute == false)
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
            return View(Model);
        }
        public IActionResult Search(SearchKeyword Search)
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            ViewBag.LogArea = Search.AreaCity;
            ViewBag.LogTown = Search.AreaTown;
            ViewBag.LogKeyword = Search.Keyword;

            var AreaModel =new List<CaseViewModel> { };
            //全部縣市 全部區域
            if (Search.AreaCity == "default")
            {
                AreaModel = _mutualBankContext.Cases.Include("CaseSkil")
                    .Where(x => x.CaseReleaseDate <= DateTime.Now && x.CaseExpireDate <= DateTime.Now.AddDays(ExpireDays) && x.CaseIsExecute == false)
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
                    }).ToList();
            }
            //縣市 全部
            else if (Search.AreaTown=="default") {
                AreaModel = _mutualBankContext.Cases.Include("CaseSkil").Include("CaseSerAreaNavigation")
                    .Where(x => x.CaseSerAreaNavigation.AreaCity==Search.AreaCity
                    && x.CaseReleaseDate <= DateTime.Now && x.CaseExpireDate <= DateTime.Now.AddDays(ExpireDays) && x.CaseIsExecute == false)
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
                    }).ToList();
            }
            //區域
            else
            {
                var SearchAreaId = _mutualBankContext.Areas.Where(x => x.AreaCity == Search.AreaCity && x.AreaTown == Search.AreaTown).FirstOrDefault().AreaId;
                AreaModel = _mutualBankContext.Cases.Include("CaseSkil")
                    .Where(x => x.CaseSerArea== SearchAreaId
                    && x.CaseReleaseDate <= DateTime.Now && x.CaseExpireDate <= DateTime.Now.AddDays(ExpireDays) && x.CaseIsExecute == false)
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
                    }).ToList();
            }

            var Model = new List<CaseViewModel> { };
            if (Search.Keyword == null)
            {
                Model = AreaModel;
            }
            else 
            {
                foreach (var c in AreaModel)
                {
                    if (c.CaseTitle.Contains(Search.Keyword) | c.CaseIntroduction.Contains(Search.Keyword) | c.CaseSkillName.Contains(Search.Keyword))
                    {
                        Model.Add(c);
                    }
                }

            }
            return View("Index", Model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProfilePage1()
        {
            return View();
        }
        public IActionResult ProfilePageAjax()
        {
            return View();
        }

        public IActionResult ProfileUpdate()
        {
            return View();
        }
        public IActionResult ProfileUpdate1()
        {
            return View();
        }
    }
}