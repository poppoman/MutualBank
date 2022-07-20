using Microsoft.AspNetCore.Mvc;
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
            var Model = _mutualBankContext.Cases.ToList();
            return View(Model);
        }
        public IActionResult Search(SearchKeyword Search)
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            var Model = new List<Case> { };

            var AreaId = -1;
            if (Search.AreaTown == null | Search.AreaTown=="區域")
            {
                AreaId = _mutualBankContext.Areas.Where(x => x.AreaCity == Search.AreaCity).Select(x => x.AreaId).FirstOrDefault();
            }
            else 
            {
                AreaId = _mutualBankContext.Areas.Where(x => x.AreaTown == Search.AreaTown).Select(x => x.AreaId).FirstOrDefault();
            }
            var AreaCase = _mutualBankContext.Cases.Where(x => x.CaseSerArea == AreaId).ToList();

            if (Search.Keyword == null)
            {
                Model = AreaCase;
            }
            else 
            {
                foreach (var c in AreaCase)
                {
                    if (c.CaseTitle.Contains(Search.Keyword) | c.CaseIntroduction.Contains(Search.Keyword))
                    {
                        Model.Add(c);
                    }
                }
            }

            return View("Index", Model);
        }

        [HttpGet]
        public String GetSkillName(int SkillId)
        {
            var TagName = _mutualBankContext.Skills.Where(x => x.SkillId == SkillId).Select(x => x.SkillName).FirstOrDefault();
            if (TagName == null)
            {
                TagName = "未分類";
            }
            return TagName;
        }
        

        //篩選功能
        //根據需求或技能
        [HttpGet]
        public List<Case> GetTypeModel(bool Bit)
        {
            var Model = _mutualBankContext.Cases.Where(x => x.CaseNeedHelp == Bit).ToList();
            return Model;
        }
        //根據技能標籤
        [HttpGet]
        public List<Case> GetTagModel(int SkillId)
        {
            var Model = _mutualBankContext.Cases.Where(x => x.CaseSkilId == SkillId).ToList();
            return Model;
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
    }
}