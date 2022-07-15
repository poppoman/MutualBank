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
            //首頁初始資料
            //分類標籤
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            //縣市
            ViewBag.City = _mutualBankContext.Areas.Select(x => x.AreaCity).Distinct().ToList();

            return View();
        }


        [HttpGet]
        public IActionResult InitCaseModel() {
            var Model = _mutualBankContext.Cases.ToList();
            return PartialView("_CasePartial", Model);
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
        [HttpGet]
        public List<string> GetTown(string AreaCity)
        {
            //鄉鎮區
            var Town = _mutualBankContext.Areas.
                Where(x => x.AreaCity == AreaCity).Select(x => x.AreaTown).ToList();

            return Town;
        }

        //篩選功能
        //根據需求或技能
        [HttpGet]
        public IActionResult GetTypeModel(bool bit)
        {
            var Model = _mutualBankContext.Cases.Where(x => x.CaseNeedHelp == bit).ToList();
            return PartialView("_CasePartial", Model);
        }
        //根據技能標籤
        [HttpGet]
        public IActionResult GetTagModel(int SkillId)
        {
            var Model = _mutualBankContext.Cases.Where(x => x.CaseSkilId == SkillId).ToList();
            return PartialView("_CasePartial", Model);
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