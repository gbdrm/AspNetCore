using System;
using System.Linq;
using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Next(Guid questionId, Guid gameId, string answer)
        {
            if (questionId == default(Guid) && gameId == default(Guid)) return BadRequest();
            if (answer == null) answer = string.Empty;
            var package = _context.TestPackages.Include(p => p.TestItems).SingleOrDefault(p => p.TestPackageId == gameId);
            var current = _context.TestItems.Single(i => i.TestItemId == questionId);

            var userId = Helper.GetCurrentUserId(User, Request.Cookies["userId"]);
            if (userId == null)
            {
                var user = new ApplicationUser();

                _context.Add(user);
                _context.SaveChanges();

                userId = user.Id;
                Response.Cookies.Append("userId", userId.ToString());
            }

            var answerDb = _context.Answers.SingleOrDefault(a => a.UserId == userId && a.TestItemId == questionId);
            if (answerDb == null)
            {
                _context.Add(new Answer
                {
                    Value = answer,
                    TestItemId = questionId,
                    UserId = userId,
                    IsCorrect = answer.ToLower() == current.Answer
                });
            }
            else
            {
                answerDb.Value = answer;
                answerDb.IsCorrect = answer.ToLower() == current.Answer;
            }

            var nextQuestion = package.TestItems.OrderBy(i => i.Order)
                .FirstOrDefault(i => i.Order > current.Order);
            _context.SaveChanges();

            if (nextQuestion == null)
            {
                var allAnswers = _context.Answers
                    .Where(a => a.UserId == userId && a.TestItem.TestPackageId == gameId)
                    .Include(a => a.TestItem);
                var correctAnswers = allAnswers.Where(a => a.IsCorrect);
                double score = (double)correctAnswers.Count() / allAnswers.Count();

                var finishModel = new TestResult
                {
                    TestPackageId = gameId,
                    TestPackage = package,
                    UserId = userId,
                    Score = score,
                };

                ViewBag.Answers = allAnswers;

                return View("Finish", finishModel);
            }

            return View("Index", nextQuestion);
        }

        public IActionResult Index(Guid? gameId = null)
        {
            var package = _context.TestPackages.Include(p => p.TestItems).SingleOrDefault(p => p.TestPackageId == gameId);

            return View(package.TestItems.OrderBy(p => p.Order).FirstOrDefault());
        }

        public IActionResult Start()
        {
            var firstTest = _context.TestPackages.Include(p => p.TestItems).FirstOrDefault();
            if (firstTest?.TestItems != null)
            {
                var firstTask = firstTest.TestItems.FirstOrDefault();
                return View("Index", firstTask);
            }

            return View("Index");
        }
    }
}