using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly MutualBankContext _MutualBankContext;
        public HomeController(MutualBankContext MutualBankContext)
        {
            _MutualBankContext = MutualBankContext;

        }

        //[Authorize]
        public IActionResult Index()
        {
            //註冊人數統計
            string[] Months = { "1月", "2月", "3月" , "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
            ViewBag.Months = Newtonsoft.Json.JsonConvert.SerializeObject(Months);
            List<int> userslist = new List<int>();
            var usersCount = _MutualBankContext.ReguserReports.Select(x => new 
            { 
                lable = x.Month,
                data = x.People,
                
            }).ToList();


            foreach (var item in usersCount) 
            {
                
                userslist.Add((int)item.data);
            }
            ViewBag.usersdata = Newtonsoft.Json.JsonConvert.SerializeObject(userslist);
           
            
            //文章數量
            List<int> caselist = new List<int>();
            var casedata = _MutualBankContext.RegcaseReports.Where(x=>x.Year==2022).Select(x => new
            {
                lable = x.Month,
                data = x.CaseNum,
            }).ToList();
            int[] arry = new int[13];
            foreach (var item in casedata) {
                arry[(int)item.lable] = (int)item.data;
            }

            ViewBag.casedata = Newtonsoft.Json.JsonConvert.SerializeObject(arry);


            //文章技能統計
            var skillname = _MutualBankContext.Skills.Select(x => x.SkillName).ToList();
            ViewBag.skillnamedata = Newtonsoft.Json.JsonConvert.SerializeObject(skillname);
            List<int> skillList = new List<int>();
            var skilldata = _MutualBankContext.Cases.Include("CaseSkil").GroupBy(z => z.CaseSkil.SkillName).Select(x => new
            {
                lable = x.Key,
                data = x.Count(),

            }).ToList();

            foreach (var item in skilldata)
            {
                skillList.Add(item.data);
            }
            ViewBag.skilldata = Newtonsoft.Json.JsonConvert.SerializeObject(skillList);

            //文章類型統計
            List<int> skilltypeList = new List<int>();
            


            return View();
        }
       
    }
}
