using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MutualBank.Extensions;
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
            //留言爸爸
            ViewBag.messages = _mutualBankContext.Messages.Include("MsgCase").Include("MsgUser").
                Where(c => c.MsgCaseId == id && c.MsgParentId==null).Select(x => new MessageVM{ 
                    MsgCaseId = x.MsgCaseId,
                    MsgAddDate = x.MsgAddDate,
                    MsgContent = x.MsgContent,
                    MsgUserId = x.MsgUserId,
                    MsgtoUserName = x.MsgToUser.UserNname, /*被留言者*/
                    MsgUserName = x.MsgUser.UserNname,/*留言者*//*$"{x.MsgUser.UserFname}{x.MsgUser.UserLname}"*/
                    MagUserPhoto = x.MsgUser.UserHphoto, /*留言者照片*/
                    
                }).ToList();
            //回覆小孩
            ViewBag.msgChild = _mutualBankContext.Messages.Include("MsgCase").Include("MsgUser").
                Where(c=>c.MsgCaseId == id && c.MsgParentId!=null).Select(x=> new MessageChVM { 
                    MsgCaseId = x.MsgCaseId,
                    MsgParentId = x.MsgParentId,
                    MsgAddDate = x.MsgAddDate,
                    MsgContent = x.MsgContent,
                    MsgUserId = x.MsgUserId,
                    MsgtoUserName = x.MsgToUser.UserNname,
                    MsgUserName = x.MsgUser.UserNname,
                    MagUserPhoto = x.MsgUser.UserHphoto,                   
                }).ToList();
             //= Newtonsoft.Json.JsonConvert.SerializeObject(messagedata);
            if (id == null)
            {
                return NotFound();
            }
            var Case = await _mutualBankContext.Cases.
                Include("CaseSkil").Include("CaseSerAreaNavigation").
                Include("CaseUser").Include("Messages").FirstOrDefaultAsync(m => m.CaseId == id);
            if (Case == null)
            {
                return NotFound();
            }

            var skillname = Case.CaseSkil.SkillName;
            var Areaname = Case.CaseSerAreaNavigation == null ? "無" : Case.CaseSerAreaNavigation.AreaCity;
            var usernname = Case.CaseUser.UserNname;
            var userphoto = Case.CaseUser.UserHphoto;
            var PhotoFileFolder = Path.Combine("/Img", "CasePhoto");
            var USER = this.User.GetId();
            var LoginHPhoto = _mutualBankContext.Users.Include("UserNavigation").Include("Cases")
                .Include("MessageMsgToUsers").Include("MessageMsgUsers").Where(x => x.UserId == USER).Select(c => c.UserFname).First();

            PostPageVM vm = new PostPageVM()
            {
                CaseId = Case.CaseId,
                CaseTitle = Case.CaseTitle,
                CasePhoto = Path.Combine(PhotoFileFolder,Case.CasePhoto),
                CasesAddDate = Case.CaseAddDate,
                CaseSerDate = Case.CaseSerDate == null ? "無" : Case.CaseSerDate,
                CaseIntroduction = Case.CaseIntroduction,
                SkillName = skillname,
                UserNName = usernname,
                UserPhoto = userphoto == null ? "~/postpage/userhphotonull.svg" : userphoto,
                Areacity = Areaname,
                LoginHPhoto = LoginHPhoto,

            };
            return View(vm);

        }

        public ActionResult Comment(int caseid)
        {
            List<Message> messages = new List<Message>();
            messages = (
                from c in _mutualBankContext.Messages
                where c.MsgId==caseid
                orderby c.MsgId descending
                select c).ToList();

            return View(messages);

        }

        [HttpPost]
        //[Authorize]
        public async Task<bool> AddComment(int id, string content)
        {
            var USER = this.User.GetId();
            var Case = await _mutualBankContext.Cases.
               Include("CaseSkil").Include("CaseSerAreaNavigation").
               Include("CaseUser").FirstOrDefaultAsync(m => m.CaseId == id);
            if (Case == null)
            {
                return false;
            }
            var comment = new Message()
            {
                MsgAddDate = DateTime.Now,
                MsgContent = content,
                MsgCaseId = Case.CaseId,
                MsgUserId = USER,
                MsgToUserId = Case.CaseUserId,
                MsgParentId = null,
                MsgIsRead = false,
   
            };
            _mutualBankContext.Add(comment);
            await _mutualBankContext.SaveChangesAsync();

            return true;
        }
    }
}
  
