using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            return _context.Points != null ?
                                    View( _context.Points) :
                                    Problem("Entity set 'MutualBankContext.Points'  is null.");
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
