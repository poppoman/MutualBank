using Microsoft.AspNetCore.Mvc;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    public class CaseController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;
        public CaseController(MutualBankContext MutualBankContext)
        {
            _mutualBankContext = MutualBankContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetPartialPostCase() 
        {
            return PartialView("PostCase");
        }
        public IActionResult GetPartialCaseList()
        {
            return PartialView("CaseList");
        }
    }
}
