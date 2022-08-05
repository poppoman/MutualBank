using Microsoft.AspNetCore.Mvc;
using MutualBank.Extensions;
using MutualBank.Models;
using MutualBank.Models.ViewModels;

namespace MutualBank.Controllers
{
    [Route("api/Users1Controller/{action}")]
    [ApiController]
    public class Users1Controller : ControllerBase
    {
        private readonly MutualBankContext _mutualBankContext;

        public Users1Controller(MutualBankContext mutualBankContext)
        {
            _mutualBankContext = mutualBankContext;
        }

        [HttpGet]
        public MemberInfo getUserInfo()
        {
            var userid = this.User.GetId();
            var userInfo = _mutualBankContext.Users.FirstOrDefault(x => x.UserId == userid);
            var area = _mutualBankContext.Areas.FirstOrDefault(x => userInfo.UserAreaId == x.AreaId);
            MemberInfo memberInfo = new MemberInfo
            {
                UserFname = userInfo.UserFname,
                UserCv = userInfo.UserCv,
                UserEmail = userInfo.UserEmail,
                UserLname = userInfo.UserLname,
                UserNname = userInfo.UserNname,
                UserResume = userInfo.UserResume,
                UserSchool = userInfo.UserSchool,
                UserSex = userInfo.UserSex,
                UserAreaId = area,
                UserSkillId = userInfo.UserSkillId,
                UserBirthday = Convert.ToDateTime(userInfo.UserBirthday).ToString("yyyy-MM-dd")
            };

            return memberInfo;
        }

        [HttpGet]
        public List<SkillInfo> getSkillInfo()
        {

            var skillid = _mutualBankContext.Skills.Select(x => x.SkillId).ToList();
            var skillname = _mutualBankContext.Skills.Select(x => x.SkillName).ToList();
            List<SkillInfo> Info = new List<SkillInfo>();
            for (int i = 0; i < skillid.Count; i++)
            {
                SkillInfo skillInfo = new SkillInfo
                {
                    SkillId = skillid[i],
                    SkillName = skillname[i],
                };
                Info.Add(skillInfo);
            }
            return Info;
        }

        [HttpGet]
        public List<string> getTempTown()
        {
            var userid = this.User.GetId();
            var areaid = _mutualBankContext.Users.Where(x => x.UserId == userid).Select(x => x.UserAreaId).FirstOrDefault();
            var areacity = _mutualBankContext.Areas.Where(x => x.AreaId == areaid).Select(x => x.AreaCity).Distinct().FirstOrDefault();
            var temptown = _mutualBankContext.Areas.Where(x => x.AreaCity == areacity).Select(x => x.AreaTown).ToList();
            return temptown;
        }
    }
}
