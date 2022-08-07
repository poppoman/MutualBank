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
    [Route("Admin/[controller]")]
    public class SkillsController : Controller
    {
        private readonly MutualBankContext _context;

        public SkillsController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Skills
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var skillModel = _context.Skills.Select(s => new SkillsIndex
            {
                SkillId = s.SkillId,
                SkillName = s.SkillName,
            }).ToList();
              return (skillModel != null) ? 
                     View("Index",skillModel) :Problem("找不到資料");
        }

        // GET: Admin/Skills/Create
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Skills/Create        
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SkillId,SkillName")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        // GET: Admin/Skills/Edit/5
        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute(Name = "id")]int? id)
        {
            if (id == null || _context.Skills == null)
            {
                return NotFound();
            }
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            var skillModel = new SkillsIndex
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName,
            };
            ViewBag.skillId = id;
            ViewBag.skillName = skillModel.SkillName;
            return View(skillModel);
        }

        // POST: Admin/Skills/Edit/5        
        [HttpPost]
        [Route("Edit/{id}")]
        [Produces("application/json")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [FromForm]SkillsIndex json)
        {
            var skillModel = _context.Skills.Where(u => u.SkillId == id).FirstOrDefault();
            if ( !SkillExists(id) || skillModel == null)
            {
                return NotFound();
            }
            try
            {
                _context.Skills.Update(CorrespondTheValue(skillModel, json));
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }        

        private bool SkillExists(int id)
        {
          return (_context.Skills?.Any(e => e.SkillId == id)).GetValueOrDefault();
        }
        private Skill CorrespondTheValue(Skill skillModel, SkillsIndex json)
        {
            skillModel.SkillName = json.SkillName;
            return skillModel;
        }
    }
}
