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

        public HomeController(ILogger<HomeController> logger, MutualBankContext mutualBankContext)
        {
            _logger = logger;
            _mutualBankContext = mutualBankContext;
        }

        public IActionResult Index()
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            var Model = _mutualBankContext.Cases.Include("CaseSkil")
                .Select(x => new CaseViewModel
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
                    CaseUserName = $"{x.CaseUser.UserLname}{x.CaseUser.UserFname}"

                });
            return View(Model);
        }
        public IActionResult Search(SearchKeyword Search)
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            ViewBag.Area = $"{Search.AreaCity} {Search.AreaTown}";
            var Model = new List<CaseViewModel>{ };

            var AreaId = -1;
            if (Search.AreaTown == null | Search.AreaTown == "區域")
            {
                ViewBag.Area = $"{Search.AreaCity}";
                AreaId = _mutualBankContext.Areas.Where(x => x.AreaCity == Search.AreaCity).Select(x => x.AreaId).FirstOrDefault();
            }
            else
            {
                ViewBag.Area = $"{Search.AreaCity} {Search.AreaTown}";
                AreaId = _mutualBankContext.Areas.Where(x => x.AreaTown == Search.AreaTown).Select(x => x.AreaId).FirstOrDefault();
            }
            var AreaModel = _mutualBankContext.Cases.Include("CaseSkil").Where(x => x.CaseSerArea == AreaId).Select(x => new CaseViewModel
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
                CaseUserName = $"{x.CaseUser.UserLname}{x.CaseUser.UserFname}"

            }).ToList();

            if (Search.Keyword == null)
            {
                Model = AreaModel;
            }
            else 
            {
                foreach (var c in AreaModel)
                {
                    if (c.CaseTitle.Contains(Search.Keyword) | c.CaseIntroduction.Contains(Search.Keyword))
                    {
                        Model.Add(c);
                    }
                }

            }
            return View("Index", Model);
        }

        public string GetAllCaseModel()
        {
            var Model = _mutualBankContext.Cases.Include("CaseSkil").Include("Messages")
                .Select(x => new CaseViewModel
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
                }) ;
            var ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
            return ModelJson;
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
    }
}