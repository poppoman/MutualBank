using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CasesController : Controller
    {
        private readonly MutualBankContext _context;


        public CasesController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Cases
        public async Task<IActionResult> Index()
        {
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
                UserName = (c.CaseUser.UserFname + " " + c.CaseUser.UserLname != " ") ? 
                            (c.CaseUser.UserFname + " " + c.CaseUser.UserLname):"無名氏",
            });
            return View(query);
        }

        // GET: Admin/Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cases == null)
            {
                return NotFound();
            }
            return NotFound();
            //var @case = await _context.Cases
            //    .Include(@ => @.CaseSerAreaNavigation)
            //    .Include(@ => @.CaseSkil)
            //    .Include(@ => @.CaseUser)
            //    .FirstOrDefaultAsync(m => m.CaseId == id);
            //if (@case == null)
            //{
            //    return NotFound();
            //}

            //return View(@case);
        }

        // GET: Admin/Cases/Create
        public IActionResult Create()
        {
            ViewData["CaseSerArea"] = new SelectList(_context.Areas, "AreaId", "AreaId");
            ViewData["CaseSkilId"] = new SelectList(_context.Skills, "SkillId", "SkillId");
            ViewData["CaseUserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/Cases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseId,CaseUserId,CaseNeedHelp,CaseSkilId,CaseAddDate,CaseReleaseDate,CaseExpireDate,CaseClosedDate,CaseTitle,CaseIntroduction,CasePhoto,CaseSerDate,CaseSerArea")] Case @case)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@case);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseSerArea"] = new SelectList(_context.Areas, "AreaId", "AreaId", @case.CaseSerArea);
            ViewData["CaseSkilId"] = new SelectList(_context.Skills, "SkillId", "SkillId", @case.CaseSkilId);
            ViewData["CaseUserId"] = new SelectList(_context.Users, "UserId", "UserId", @case.CaseUserId);
            return View(@case);
        }

        // GET: Admin/Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cases == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            ViewData["CaseSerArea"] = new SelectList(_context.Areas, "AreaId", "AreaId", @case.CaseSerArea);
            ViewData["CaseSkilId"] = new SelectList(_context.Skills, "SkillId", "SkillId", @case.CaseSkilId);
            ViewData["CaseUserId"] = new SelectList(_context.Users, "UserId", "UserId", @case.CaseUserId);
            return View(@case);
        }

        // POST: Admin/Cases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseId,CaseUserId,CaseNeedHelp,CaseSkilId,CaseAddDate,CaseReleaseDate,CaseExpireDate,CaseClosedDate,CaseTitle,CaseIntroduction,CasePhoto,CaseSerDate,CaseSerArea")] Case @case)
        {
            if (id != @case.CaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseSerArea"] = new SelectList(_context.Areas, "AreaId", "AreaId", @case.CaseSerArea);
            ViewData["CaseSkilId"] = new SelectList(_context.Skills, "SkillId", "SkillId", @case.CaseSkilId);
            ViewData["CaseUserId"] = new SelectList(_context.Users, "UserId", "UserId", @case.CaseUserId);
            return View(@case);
        }

        // GET: Admin/Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cases == null)
            {
                return NotFound();
            }
            return NotFound();
            //var @case = await _context.Cases
            //    .Include(@ => @.CaseSerAreaNavigation)
            //    .Include(@ => @.CaseSkil)
            //    .Include(@ => @.CaseUser)
            //.FirstOrDefaultAsync(m => m.CaseId == id);
            //if (@case == null)
            //{
            //    return NotFound();
            //}

            //return View(@case);
        }

        // POST: Admin/Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cases == null)
            {
                return Problem("Entity set 'MutualBankContext.Cases'  is null.");
            }
            var @case = await _context.Cases.FindAsync(id);
            if (@case != null)
            {
                _context.Cases.Remove(@case);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
          return (_context.Cases?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }
    }
}
