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
            var Cards = _mutualBankContext.Cases.ToList();

            var Countys = _mutualBankContext.Areas.Select(x => x.AreaCity).Distinct().ToList();
            ViewBag.countys = Countys;

            return View(Cards);
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
        public List<string> GetTown(string AreaCity)
        {
            var Town = _mutualBankContext.Areas.
                Where(x => x.AreaCity == AreaCity).Select(x => x.AreaTown).ToList();

            return Town;
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