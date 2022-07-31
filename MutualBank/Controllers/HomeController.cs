﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Models;
using System.Diagnostics;

namespace MutualBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MutualBankContext _mutualBankContext;
        private static string _filePath= Path.Combine("/Img", "CasePhoto");
        public HomeController(ILogger<HomeController> logger, MutualBankContext mutualBankContext)
        {
            _logger = logger;
            _mutualBankContext = mutualBankContext;
        }

        public IActionResult Index()
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();

            var Model = _mutualBankContext.Cases.Include("CaseSkil").Where(x=>x.CaseClosedDate>= DateTime.Now)
                .Select(x => new CaseViewModel
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    CaseTitle = x.CaseTitle,
                    CaseIntroduction = x.CaseIntroduction,
                    CasePhoto = Path.Combine(_filePath, x.CasePhoto),
                    CaseSerDate = x.CaseSerDate,
                    CaseSerArea = x.CaseSerArea,
                    CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                    CaseSkillId = x.CaseSkil.SkillId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CaseUserId = x.CaseUser.UserId,
                    CaseUserName = x.CaseUser.UserNname,
                    MessageCount = x.Messages.Count

                });
            return View(Model);
        }
        public IActionResult Search(SearchKeyword Search)
        {
            ViewBag.Tags = _mutualBankContext.Skills.OrderBy(x => x.SkillId).ToList();
            ViewBag.LogArea = Search.AreaCity;
            ViewBag.LogTown = Search.AreaTown;
            ViewBag.LogKeyword = Search.Keyword;

            var AreaModel =new List<CaseViewModel> { };
            if (Search.AreaCity == null)
            {
                //if user did not select city
                AreaModel = _mutualBankContext.Cases.Include("CaseSkil").Where(x => x.CaseClosedDate >= DateTime.Now).Select(x => new CaseViewModel
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    CaseTitle = x.CaseTitle,
                    CaseIntroduction = x.CaseIntroduction,
                    CasePhoto = Path.Combine(_filePath, x.CasePhoto),
                    CaseSerDate = x.CaseSerDate,
                    CaseSerArea = x.CaseSerArea,
                    CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                    CaseSkillId = x.CaseSkil.SkillId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CaseUserId = x.CaseUser.UserId,
                    CaseUserName = x.CaseUser.UserNname,
                    MessageCount = x.Messages.Count
                }).ToList();
            }
            else
            {
                var AreaId = _mutualBankContext.Areas.Where(x => x.AreaTown == Search.AreaTown).Select(x => x.AreaId).FirstOrDefault();
                AreaModel = _mutualBankContext.Cases.Include("CaseSkil")
                    .Where(x => x.CaseSerArea == AreaId & x.CaseClosedDate >= DateTime.Now).Select(x => new CaseViewModel
                {
                    CaseId = x.CaseId,
                    CaseNeedHelp = x.CaseNeedHelp,
                    CaseReleaseDate = x.CaseReleaseDate,
                    CaseExpireDate = x.CaseExpireDate,
                    CaseTitle = x.CaseTitle,
                    CaseIntroduction = x.CaseIntroduction,
                    CasePhoto = Path.Combine(_filePath, x.CasePhoto),
                    CaseSerDate = x.CaseSerDate,
                    CaseSerArea = x.CaseSerArea,
                    CaseSerAreaName = $"{x.CaseSerAreaNavigation.AreaCity}{x.CaseSerAreaNavigation.AreaTown}",
                    CaseSkillId = x.CaseSkil.SkillId,
                    CaseSkillName = x.CaseSkil.SkillName,
                    CaseUserId = x.CaseUser.UserId,
                    CaseUserName = x.CaseUser.UserNname,
                    MessageCount = x.Messages.Count
                }).ToList();
            }

            var Model = new List<CaseViewModel> { };
            if (Search.Keyword == null)
            {
                Model = AreaModel;
            }
            else 
            {
                foreach (var c in AreaModel)
                {
                    if (c.CaseTitle.Contains(Search.Keyword) | c.CaseIntroduction.Contains(Search.Keyword) | c.CaseSkillName.Contains(Search.Keyword))
                    {
                        Model.Add(c);
                    }
                }

            }
            return View("Index", Model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProfilePage1()
        {
            return View();
        }
        public IActionResult ProfilePageAjax()
        {
            return View();
        }

        public IActionResult ProfileUpdate()
        {
            return View();
        }

    }
}