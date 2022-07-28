﻿using System;
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
            var messages = _mutualBankContext.Messages.Include("MsgCase").Include("MsgUser").
                    Where(c => c.MsgCaseId == id & c.MsgParentId == null).Select(x => new {
                        MsgCaseId = x.MsgCaseId,
                        MsgAddDate = x.MsgAddDate,
                        MsgContent = x.MsgContent,
                        MsgUserId = x.MsgUserId,
                        MsgtoUserName = x.MsgToUser.UserNname, /*被留言者*/
                        MsgUserName = x.MsgUser.UserNname,/*留言者*//*$"{x.MsgUser.UserFname}{x.MsgUser.UserLname}"*/
                        MagUserPhoto = x.MsgUser.UserHphoto, /*留言者照片*/
                        MsgParentId = x.MsgParentId,
                        Mychildinhouse = _mutualBankContext.Messages.Include("MsgUser").Where(q => q.MsgParentId == x.MsgId & q.MsgParentId != null).Select(y => new { 
                            childname = y.MsgUser.UserNname,
                            childtoUsername = y.MsgToUser.UserNname,
                            childHphoto = y.MsgUser.UserHphoto,
                            childtoUserHphoto = y.MsgToUser.UserHphoto,
                            childcontent = y.MsgContent,
                            childaddDate = y.MsgAddDate,
                        
                        }).ToList()

                    }).ToList();
            ViewBag.tmessages = Newtonsoft.Json.JsonConvert.SerializeObject(messages);

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
            var AreaTownname = Case.CaseSerAreaNavigation == null ? "無" : Case.CaseSerAreaNavigation.AreaTown;
            var usernname = Case.CaseUser.UserNname;
            var userphoto = Case.CaseUser.UserHphoto;
            var PhotoFileFolder = Path.Combine("/Img", "CasePhoto");
            
           
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
                AreaTownname = AreaTownname,


            };
            return View(vm);

        }
        
        
        //[Authorize]
        public async Task<IActionResult> message(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var USER = this.User.GetId();
            var LoginHPhoto = _mutualBankContext.Users.Include("UserNavigation").Include("Cases")
                .Include("MessageMsgToUsers").Include("MessageMsgUsers").Where(x => x.UserId == USER).Select(c => c.UserFname).First();
            PostPageVM vm = new PostPageVM()
            {
                LoginHPhoto = LoginHPhoto,
            };


            return View(vm);

        }


      

        [HttpPost]
        [Authorize]
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
  
