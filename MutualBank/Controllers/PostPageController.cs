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
using MutualBank.Models.ViewModels.Point;

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
                    Where(c => c.MsgCaseId == id & c.MsgParentId == null).Select(x => new
                    {
                        msgId = x.MsgId,
                        msgCaseId = x.MsgCaseId,
                        msgAddDate = x.MsgAddDate,
                        msgContent = x.MsgContent,
                        msgUserId = x.MsgUserId,
                        msgtoUserName = x.MsgToUser.UserNname, /*被留言者*/
                        msgUserName = x.MsgUser.UserNname,/*留言者*//*$"{x.MsgUser.UserFname}{x.MsgUser.UserLname}"*/
                        magUserPhoto = x.MsgUser.UserHphoto, /*留言者照片*/
                        msgParentId = x.MsgParentId,
                        mychildinhouse = _mutualBankContext.Messages.Include("MsgUser").Where(q => q.MsgParentId == x.MsgId & q.MsgParentId != null).Select(y => new
                        {
                            childname = y.MsgUser.UserNname,
                            childtoUsername = y.MsgToUser.UserNname,
                            childHphoto = y.MsgUser.UserHphoto,
                            childtoUserHphoto = y.MsgToUser.UserHphoto,
                            childcontent = y.MsgContent,
                            childaddDate = y.MsgAddDate,
                            childparentid = y.MsgParentId,
                            chileuserid = y.MsgUserId,

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
                CasePhoto = Path.Combine(PhotoFileFolder, Case.CasePhoto),
                CasesAddDate = Case.CaseAddDate,
                CaseSerDate = Case.CaseSerDate == null ? "無" : Case.CaseSerDate,
                CaseIntroduction = Case.CaseIntroduction,
                SkillName = skillname,
                UserNName = usernname,
                UserPhoto = userphoto == null ? "~/postpage/userhphotonull.svg" : userphoto,
                Areacity = Areaname,
                AreaTownname = AreaTownname,
                IsExecute = Case.CaseIsExecute,
                UserPoint = Case.CaseUser.UserPoint
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
        public ActionResult AddComment(int id, string content)
        {
            var USER = this.User.GetId();

            var Case = _mutualBankContext.Cases.
               Include("CaseSkil").Include("CaseSerAreaNavigation").
               Include("CaseUser").FirstOrDefault(m => m.CaseId == id);
            var User = _mutualBankContext.Users.Include("User1").Include("UserNavigation").Include("UserSkill").Where(x => x.UserId == USER).First();

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
            _mutualBankContext.SaveChangesAsync();

            msgnewVM vm = new msgnewVM()
            {
                MagUserPhoto = User.UserHphoto,
                MsgAddDate = comment.MsgAddDate,
                MsgCaseId = comment.MsgCaseId,
                MsgContent = comment.MsgContent,
                MsgParentId = comment.MsgParentId,
                MsgUserId = comment.MsgUserId,
                MsgUserName = User.UserNname,
                MsgtoUserName = Case.CaseUser.UserNname,
            };


            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddreComment(int id, string content, int parentid, int touserid)
        {
            var USER = this.User.GetId();
            var Case = _mutualBankContext.Cases.
               Include("CaseSkil").Include("CaseSerAreaNavigation").
               Include("CaseUser").FirstOrDefault(m => m.CaseId == id);
            var User = _mutualBankContext.Users.Include("User1").Include("UserNavigation").Include("UserSkill").Where(x => x.UserId == USER).First();
            var toUser = _mutualBankContext.Users.Include("User1").Include("UserNavigation").Include("UserSkill").Where(x => x.UserId == touserid).First();

            var recomment = new Message()
            {
                MsgAddDate = DateTime.Now,
                MsgContent = content,
                MsgCaseId = Case.CaseId,
                MsgUserId = USER,
                MsgToUserId = touserid,
                MsgParentId = parentid,
                MsgIsRead = false,
            };

            _mutualBankContext.Add(recomment);
            _mutualBankContext.SaveChangesAsync();

            msgnewVM vm = new msgnewVM()
            {
                childname = User.UserNname,
                childtoUsername = toUser.UserNname,
                childHphoto = User.UserHphoto,
                childtoUserHphoto = toUser.UserHphoto,
                childcontent = recomment.MsgContent,
                childaddDate = recomment.MsgAddDate,
                childparentid = recomment.MsgParentId,
                chileuserid = recomment.MsgUserId,

            };
            return Json(vm);
        }

        [HttpGet]
        public JsonResult GetMsgUserModel(int id)
        {
            //who leave message
            var Msgs = _mutualBankContext.Messages.Include("MsgUser")
                .Where(x => x.MsgCaseId == id & x.MsgParentId == null)
                .Select(x => new
                {
                    UserId = x.MsgUserId,
                    UserName = x.MsgUser.UserNname,
                    UserPhoto = x.MsgUser.UserHphoto,
                }).Distinct().ToList();

            var res = _mutualBankContext.Messages.Include("MsgUser").Include("MsgCase")
    .Where(x => x.MsgCaseId == id & x.MsgParentId == null)
    .Select(x => new
    {
        UserId = x.MsgUserId,
        UserName = x.MsgUser.UserNname,
        UserPhoto = x.MsgUser.UserHphoto,
        CasePoint = x.MsgCase.CasePoint,
        UserPoint = x.MsgUser.UserPoint,
        MsgList = Msgs
    }).First();

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(res));
        }

        [HttpPost]
        public IActionResult SavePointTransLog([FromBody] PointTransLog para)
        {
            var TransCase = _mutualBankContext.Cases.Find(para.CaseId);
            TransCase.CasePoint = para.CasePoint;
            TransCase.CaseIsExecute = true;
            var IsCaseNeed = TransCase.CaseNeedHelp;
            var PostId = TransCase.CaseUserId;
            var MsgId = para.TargetUserId;

            //deposit userpoints to log
            if (IsCaseNeed)
            {
                var PostOwner = _mutualBankContext.Users.Find(PostId);
                PostOwner.UserPoint -= para.CasePoint;
            }
            else
            {
                var MsgUser = _mutualBankContext.Users.Find(MsgId);
                MsgUser.UserPoint -= para.CasePoint;
            }

            for (int i = 0; i < 2; i++)
            {
                _mutualBankContext.Points.Add(new Point()
                {
                    PointAddDate = DateTime.Now,
                    PointCaseId = para.CaseId,
                    PointNeedHelp = IsCaseNeed,
                    PointUserId = IsCaseNeed ? PostId : MsgId,
                    PointQuantity = para.CasePoint
                });
                IsCaseNeed = !IsCaseNeed;
            }
            _mutualBankContext.SaveChanges();
            return Ok(200);
        }
        [HttpGet]
        public bool IsOwner(int CaseId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }
            return _mutualBankContext.Cases.FirstOrDefault(x => x.CaseId == CaseId && x.CaseUserId == User.GetId()) == null ? false : true;
        }
    }
}

