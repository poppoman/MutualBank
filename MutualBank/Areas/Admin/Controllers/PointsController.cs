using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class PointsController : Controller
    {
        private readonly MutualBankContext _context;

        public PointsController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Points
        [Route("Index")]
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
                PointUserName = p.PointCase.CaseUser.UserLname + " " + p.PointCase.CaseUser.UserFname != " " ?
                                p.PointCase.CaseUser.UserLname + " " + p.PointCase.CaseUser.UserFname : "無名氏"
            });
            return View(pModel);
        }

        //Get: Points/Edit/{PointId}
        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit([FromRoute(Name ="id")]int? id)
        {
            var pModel = _context.Points;
            if (id == null || pModel == null)
            { return NotFound(); }
            var query = _context.Points.Where(p => p.PointId == id).FirstOrDefault();
            if (query != null)
            {
                ViewBag.PointId = id;
                ViewBag.CaseId = query.PointCaseId;                
                return View(query);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("getPointDetail/{id}")]
        public IActionResult getPointDetail([FromRoute(Name ="id")]int? id)
        {
            if(id == null || _context.Points == null)
            {
                return NotFound();
            }
            var query = _context.Points.Where(p => p.PointId == id).Select(p => new PointApiModel
            {
                PointId = p.PointId,
                PointAddDate = p.PointAddDate,
                PointAddDisplay = p.PointAddDate.ToString("yyyy-MM-dd"),
                PointCaseId = p.PointCaseId,
                PointNeedHelp = p.PointNeedHelp,
                PointQuantity = p.PointQuantity,
                PointUserId = p.PointUserId
            }).FirstOrDefault();
            if (query != null)
            {
                return Ok(query);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit([FromRoute(Name ="id")]int id, [FromForm] PointApiModel json)
        {
            if (!PointExists(id))
            {
                return BadRequest();
            }            
            var pModel = _context.Points.Where(p => p.PointId == id).FirstOrDefault();
            try
            {
                _context.Points.Update(CorrespondTheValue(pModel,json));
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return BadRequest();
            }
        }

        private bool PointExists(int id)
        {
            return (_context.Points?.Any(e => e.PointId == id)).GetValueOrDefault();
        }
        private Point CorrespondTheValue(Point p, PointApiModel apiModel)
        {
            p.PointUserId = apiModel.PointUserId;
            p.PointNeedHelp = apiModel.PointNeedHelp;
            p.PointQuantity = apiModel.PointQuantity;
            return p;
        }
    }
}
