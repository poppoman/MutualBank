using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PointsController : Controller
    {
        private readonly MutualBankContext _context;

        public PointsController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Points
        public IActionResult Index()
        {
            if(_context.Points == null) 
            { return NotFound(); }
            var pModel = _context.Points.Select(p => new PointsIndex
            {
                PointId = p.PointId,
                PointAddDate = p.PointAddDate,
                PoCaseTitle = p.PointCase.CaseTitle,
                PointIsDone = p.PointIsDone,
                PointNeedDisplay = (p.PointNeedHelp.ToString() != "True") ? "提供" : "需要",
                PointQuantity = p.PointQuantity,
                PointUserName = p.PointCase.CaseUser.UserLname + " " + p.PointCase.CaseUser.UserFname,
            });
            return View(pModel);
        }

        //Get: Points/Edit/{PointId}
        public IActionResult Edit(int? id)
        {
            var pModel = _context.Points;
            if (id == null || pModel == null)
            { return NotFound(); }
            var query = _context.Points.Where(p => p.PointId == id).Select( p => new PointsIndex
            {
                PointId = p.PointId,
                PointAddDate = p.PointAddDate,
                //PointCaseId = p.PointCaseId,
                PointIsDone = p.PointIsDone,
                //PointNeedHelp = p.PointNeedHelp,
                PointQuantity = p.PointQuantity,
                PointUserId = p.PointUserId,
            });
            return View(query);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return Problem("資料錯誤，請洽系統管理員");
            }
            else
            {
                var Point = _context.Points.FirstOrDefault(p => p.PointId == id);
                return View(Point);
            }
        }

    }
}
