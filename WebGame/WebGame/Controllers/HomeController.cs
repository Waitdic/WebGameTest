using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebGame.Models;


namespace WebGame.Controllers
{
    public class HomeController : Controller
    {
        AnswerContext db;

        public HomeController(AnswerContext context)
        {
            db = context;
        }

        [HttpPost]
        public IActionResult Index(Answer answer)
        {
            MakeSystemAnswer sysAns = new MakeSystemAnswer(ref db);
            answer.SystemAnswer = sysAns.MakingSystemAnswer(answer);

            db.Answers.Add(answer);
            db.SaveChanges();
            return View(db.Answers.ToList());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(db.Answers.ToList());
        }       
    }
}
