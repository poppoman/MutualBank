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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Users1Controller(MutualBankContext mutualBankContext , IWebHostEnvironment webHostEnvironment)
        {
            _mutualBankContext = mutualBankContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public MemberInfo getUserInfo()
        {
            var PhotoFileFolder = Path.Combine("/Img", "User");
            var userid = this.User.GetId();
            var userInfo = _mutualBankContext.Users.FirstOrDefault(x => x.UserId == userid);
            var area = _mutualBankContext.Areas.FirstOrDefault(x => userInfo.UserAreaId == x.AreaId);
            Area a = new Area { AreaCity = "臺北市", AreaId = 1, AreaTown = "中正區" };
            if (userInfo.UserFname == null) userInfo.UserFname = "";
            if (userInfo.UserLname == null) userInfo.UserLname = "";
            if (userInfo.UserCv == null) userInfo.UserCv = "";
            if (userInfo.UserResume == null) userInfo.UserResume = "";
            if (userInfo.UserSchool == null) userInfo.UserSchool = "";
            if (userInfo.UserBirthday == null) userInfo.UserBirthday = Convert.ToDateTime("1970-01-01");
            if (userInfo.UserHphoto == null) userInfo.UserHphoto = Path.Combine(PhotoFileFolder, "Male.PNG");
            if (area == null) area = a;
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
                UserBirthday = Convert.ToDateTime(userInfo.UserBirthday).ToString("yyyy-MM-dd"),
                UserHphoto = Path.Combine(PhotoFileFolder, userInfo.UserHphoto),
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
            if (areaid != null)
            {
                var areacity = _mutualBankContext.Areas.Where(x => x.AreaId == areaid).Select(x => x.AreaCity).Distinct().FirstOrDefault();
                var temptown = _mutualBankContext.Areas.Where(x => x.AreaCity == areacity).Select(x => x.AreaTown).ToList();
                return temptown;
            }
            else 
            {
                var areacity = _mutualBankContext.Areas.Where(x => x.AreaId == 1).Select(x => x.AreaCity).Distinct().FirstOrDefault();
                var temptown = _mutualBankContext.Areas.Where(x => x.AreaCity == areacity).Select(x => x.AreaTown).ToList();
                return temptown;
            } 
        }
        [HttpGet]
        public List<Sex> getSex()
        {
            List<bool> si = new List<bool>();
            si.Add(true); si.Add(false);
            List<string> sn = new List<string>();
            sn.Add("男"); sn.Add("女");
            List<Sex> sex = new List<Sex>();
            for (int i = 0; i < si.Count; i++)
            {
                Sex s = new Sex();
                s.SexId = si[i];
                s.SexName = sn[i];
                sex.Add(s);
            }
            return sex;
        } 
    }
}
