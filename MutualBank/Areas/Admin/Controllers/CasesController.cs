using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class CasesController : Controller
    {
        private readonly MutualBankContext _context;


        public CasesController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Cases
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            if (_context.Cases == null)
            { return NotFound(); }
            var query = _context.Cases.Include(AreaNav => AreaNav.CaseSerAreaNavigation).Include(skil => skil.CaseSkil).Include(user => user.CaseUser).Select(c => new CaseIndex
            {
                CaseClosedIn = c.CaseClosedDate ?? new DateTime(2022,08,06),
                CaseExpireDate = c.CaseExpireDate,
                CaseId = c.CaseId,
                CaseIsExecute = c.CaseIsExecute,
                CaseNeedHelp = c.CaseNeedHelp,
                CasePoint = c.CasePoint,
                CaseReleaseDate = c.CaseReleaseDate.Date,
                CaseTitle = c.CaseTitle,
                SerArea = c.CaseSerAreaNavigation.AreaCity + c.CaseSerAreaNavigation.AreaTown,
                CaseSerDate = c.CaseSerDate,
                CaseSkil = c.CaseSkil.SkillName,
                CaseUserId = c.CaseUserId,
                UserName = (c.CaseUser.UserLname + " " + c.CaseUser.UserFname != " ") ? 
                            (c.CaseUser.UserLname + " " + c.CaseUser.UserFname):"無名氏",
            });
            return View(query);
        }

        // GET: Admin/Cases/Edit/5
        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute(Name ="id")]int? id)
        {
            if (id == null || _context.Cases == null)
            {
                return NotFound();
            }

            var caseModel = await _context.Cases.FindAsync(id);
            if (caseModel == null)
            {
                return NotFound();
            }
            ViewBag.CaseId = id;
            ViewBag.AreaName = _context.Areas.Where(a => a.AreaId == caseModel.CaseSerArea).Select(a => new
            {
                areaName = a.AreaCity + a.AreaTown,
            }).FirstOrDefault();
            ViewBag.title = caseModel.CaseTitle;
            return View(caseModel);            
        }

        // POST: Admin/Cases/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Edit(int id, [FromForm]CaseApiModel json)
        {
            var caseModel = _context.Cases.Where(c => c.CaseId == id).FirstOrDefault();
            if (caseModel == null || json == null)
            {
                return NotFound();
            }
            try
            {
                _context.Update(CorrespondTheValue(caseModel, json));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(caseModel.CaseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet]
        [Route("getReadonlyCases/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Case), 200)]
        [ProducesResponseType(typeof(ApiMsg), 400)]
        public IActionResult getReadonlyCases([FromRoute(Name = "id")] int id)
        {
            var caseModel = _context.Cases.Where(c => c.CaseId == id).Select(c => new CaseApiModel
            {
                CaseAddString = c.CaseAddDate.ToString("yyyy-MM-dd"),
                CaseClosedIn = c.CaseClosedDate ?? new DateTime(2022, 08, 06),
                CaseExpireString = c.CaseExpireDate.ToString("yyyy-MM-dd"),
                CaseIntroduction = c.CaseIntroduction,
                CaseNeedHelp = c.CaseNeedHelp,
                CaseId = c.CaseId,
                CasePoint = c.CasePoint,
                CaseReleaseString = c.CaseReleaseDate.ToString("yyyy-MM-dd"),
                CaseSerArea = c.CaseSerArea,
                CaseSerDate = c.CaseSerDate,
                CaseSkilId = c.CaseSkilId,
                CaseTitle = c.CaseTitle,
                CaseUserId = c.CaseUserId,
                userFullName = c.CaseUser.UserLname + " " + c.CaseUser.UserFname != " " ?
                               c.CaseUser.UserLname + " " + c.CaseUser.UserFname : c.CaseUserId.ToString()
            }).FirstOrDefault();
            if (caseModel == null)
            {
                return this.StatusCode(400, errorMsg());
            }            
            return this.StatusCode(200, caseModel);
        }

        [HttpGet]
        [Route("getAreaList")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Area), 200)]
        [ProducesResponseType(typeof(ApiMsg), 400)]
        public IActionResult getAreaList()
        {
            var areaModel = _context.Areas;
            if (areaModel == null)
            {
                return this.StatusCode(400, errorMsg());
            }
            return this.StatusCode(200, CreateAreaView());
        }

        [HttpGet]
        [Route("getSkillList")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SkillsIndex), 200)]
        [ProducesResponseType(typeof(ApiMsg), 400)]
        public IActionResult getSkillList()
        {
            var skill = _context.Skills;
            if (skill == null)
            {
                return this.StatusCode(400, errorMsg());
            }
            return this.StatusCode(200, CreateSkillView());

        }

        [HttpGet]
        [Route("getTitle/{id}")]
        [Produces("application/json")]
        public IActionResult getTitle([FromRoute(Name ="id")]int id)
        {
            var cModel = _context.Cases.Where(c => c.CaseId == id).Select(c=> new
            {
                title = c.CaseTitle
            }).FirstOrDefault();
            return Ok(cModel);
        }

        private bool CaseExists(int id)
        {
          return (_context.Cases?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }

        private Case CorrespondTheValue(Case caseModel, CaseApiModel apiModel)
        {
            caseModel.CaseExpireDate = apiModel.CaseExpireDate;
            caseModel.CaseIntroduction = apiModel.CaseIntroduction;
            caseModel.CaseNeedHelp = apiModel.CaseNeedHelp;
            caseModel.CasePoint = apiModel.CasePoint;
            caseModel.CaseSerDate = apiModel.CaseSerDate;
            caseModel.CaseSkilId = apiModel.CaseSkilId;
            caseModel.CaseTitle = apiModel.CaseTitle;
            caseModel.CaseUserId = apiModel.CaseUserId;
            return caseModel;
        }
        private CaseApiModel CorrespondToViewModel(Case caseModel)
        {
            var apiModel = new CaseApiModel();
            apiModel.CaseAddDate = caseModel.CaseAddDate;
            apiModel.CaseClosedDate = caseModel.CaseClosedDate;
            apiModel.CaseExpireDate = caseModel.CaseExpireDate; 
            apiModel.CaseIntroduction = caseModel.CaseIntroduction;
            apiModel.CaseNeedHelp = caseModel.CaseNeedHelp;
            apiModel.CaseId = caseModel.CaseId;
            apiModel.CasePoint = caseModel.CasePoint;
            apiModel.CaseReleaseDate = caseModel.CaseReleaseDate;
            apiModel.CaseSerArea = caseModel.CaseSerArea;
            apiModel.CaseSerDate = caseModel.CaseSerDate;
            apiModel.CaseSkilId = caseModel.CaseSkilId;
            apiModel.CaseTitle = caseModel.CaseTitle;
            apiModel.CaseUserId = caseModel.CaseUserId;           
            return apiModel;
        }
        private List<SkillsIndex> CreateSkillView()
        {
            List<SkillsIndex> skills = _context.Skills.Select(sk => new SkillsIndex
            {
                SkillId = sk.SkillId,
                SkillName = sk.SkillName,
            }).ToList();
            return skills;
        }
        private List<AreaViewModel> CreateAreaView()
        {
            List<AreaViewModel> areas = _context.Areas.Select(a => new AreaViewModel
            {
                AreaId = a.AreaId,
                AreaCity = a.AreaCity,
                AreaTown = a.AreaTown,
                SerAreaDisplay = a.AreaCity + a.AreaTown,
            }).ToList();
            return areas;
        }
        private ApiMsg errorMsg()
        {
            return new ApiMsg
            {
                code = 400,
                msg = "查無此筆資料"
            };
        }

    }
}
