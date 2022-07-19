using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;
using MutualBank.Models.ViewModels;

namespace MutualBank.Controllers
{
    public class PostPageController : Controller
    {
        private readonly MutualBankContext _mutualBankContext;

        public PostPageController(MutualBankContext MutualBankContext)
        {
            _mutualBankContext = MutualBankContext;
        }

       
        // GET: PostPage
        public async Task<IActionResult> Index(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var Case = await _mutualBankContext.Cases
            .FirstOrDefaultAsync(m => m.CaseId == id);
            if (Case == null)
            {
                return NotFound();
            }
            return View(Case);
        }

        [HttpGet]
        public List<Case> InitCaseModel()
        {
            var Model = _mutualBankContext.Cases.ToList();
            return Model;
        }

        [HttpGet]
        public String GetSkillName(int SkillId)
        {
            var TagName = _mutualBankContext.Skills
                .Where(x => x.SkillId == SkillId)
                .Select(x => x.SkillName).FirstOrDefault();
            if (TagName == null)
            {
                TagName = "未分類";
            }
            return TagName;
        }




        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddComment(string content)
        {
            PostPageViewModel dvm = new PostPageViewModel();
            //var userid = HttpContext.User.Identity.;
            var comment = new Message()
            {
                MsgAddDate = DateTime.Now,
                MsgContent = content,
            };
            _mutualBankContext.Add(comment);
            await _mutualBankContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }


    //// GET: PostPage/Details/5
    //public async Task<IActionResult> Details()
    //{

    //}

    //// GET: PostPage/Create
    //public IActionResult Create()
    //{
    //    return View();
    //}


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("MsgId,MsgAddDate,MsgCaseId,MsgUserId,MsgContent")] Message message)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Add(message);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(message);
}

        //// GET: PostPage/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Messages == null)
        //    {
        //        return NotFound();
        //    }

        //    var message = await _context.Messages.FindAsync(id);
        //    if (message == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(message);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("MsgId,MsgAddDate,MsgCaseId,MsgUserId,MsgContent")] Message message)
        //{
        //    if (id != message.MsgId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(message);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MessageExists(message.MsgId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(message);
        //}

        //// GET: PostPage/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Messages == null)
        //    {
        //        return NotFound();
        //    }

        //    var message = await _context.Messages
        //        .FirstOrDefaultAsync(m => m.MsgId == id);
        //    if (message == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(message);
        //}

        //// POST: PostPage/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Messages == null)
        //    {
        //        return Problem("Entity set 'MutualBankContext.Messages'  is null.");
        //    }
        //    var message = await _context.Messages.FindAsync(id);
        //    if (message != null)
        //    {
        //        _context.Messages.Remove(message);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MessageExists(int id)
        //{
        //  return (_context.Messages?.Any(e => e.MsgId == id)).GetValueOrDefault();
        //}
  
