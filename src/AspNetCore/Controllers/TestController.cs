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
            var isCorrect = answer.ToLower() == current.Answer.ToLower();
            if (answerDb == null)
            {
                _context.Add(new Answer
                {
                    Value = answer,
                    TestItemId = questionId,
                    UserId = userId,
                    IsCorrect = isCorrect
                });
            }
            else
            {
                answerDb.Value = answer;
                answerDb.IsCorrect = isCorrect;
            }

            var nextQuestion = package.TestItems.OrderBy(i => i.Order)
                .FirstOrDefault(i => i.Order > current.Order);
            _context.SaveChanges();

            if (nextQuestion == null)
            {
                return FinishPage(gameId, userId, package);
            }

            return View("Index", nextQuestion);
        }

        private IActionResult FinishPage(Guid gameId, Guid? userId, TestPackage package)
        {
            var allAnswers = _context.Answers
                .Where(a => a.UserId == userId && a.TestItem.TestPackageId == gameId)
                .Include(a => a.TestItem).ToList();
            var correctAnswers = allAnswers.Where(a => a.IsCorrect);
            double score = (double) correctAnswers.Count()/allAnswers.Count;

            var finishModel = new TestResult
            {
                TestPackageId = gameId,
                TestPackage = package,
                UserId = userId,
                Score = score,
            };

            //var comments = _context.Comments.Where(c => c.TestPackageId == gameId).Include(c => c.User).ToList();
            //ViewBag.Comments = comments;
            ViewBag.Answers = allAnswers;

            return View("Finish", finishModel);
        }

        [HttpPost]
        public IActionResult AddComment(Guid gameId, string commentText)
        {
            var currentUserId = Helper.GetCurrentUserId(User, Request.Cookies["userId"]);
            if (!currentUserId.HasValue)
            {
                return BadRequest();
            }

            var comment = new Comment
            {
                TestPackageId = gameId,
                Text = commentText,
                Time = DateTime.Now,
                UserId = currentUserId.Value
            };

            _context.Add(comment);
            _context.SaveChanges();

            var package = _context.TestPackages.Include(p => p.TestItems).SingleOrDefault(p => p.TestPackageId == gameId);
            return FinishPage(gameId, currentUserId, package);
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