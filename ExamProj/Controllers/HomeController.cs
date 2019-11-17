using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamProj.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User register)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == register.Email))
                {
                    ModelState.AddModelError("Email", "That email has already been registered!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    register.Password = Hasher.HashPassword(register, register.Password);

                    dbContext.Users.Add(register);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetString("UserEmail", register.Email);
                    HttpContext.Session.SetString("UserName", $"{register.FirstName} {register.LastName}");
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult LogIn(LoginUser login)
        {
            if (ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == login.LoginEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("SignIn");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(login, userInDb.Password, login.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Pasword");
                    return View("SignIn");

                }
                HttpContext.Session.SetString("UserEmail", login.LoginEmail);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("SignIn");
            }
        }


        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        ///////////////////////////////////////////// OCCASION EVENTS /////////////////////////////////////////////////

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
            if (userInDb == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("LogOut");
            }
            else
            {
                User userinDb = dbContext.Users.Include(u => u.PlannedOccasion).FirstOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userinDb;
                List<Occasion> AllOccasions = dbContext.Occasions.Include(u => u.Participants).ThenInclude(a => a.Participant).Include(o => o.Coordinator).OrderBy(d=>d.Date).ToList();
                return View(AllOccasions);
            }

        }

        [HttpGet("new")]
        public IActionResult New()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                User userinDb = dbContext.Users.Include(u => u.PlannedOccasion).FirstOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userinDb;
                return View();
            }
        }

        [HttpPost("create")]
        public IActionResult Create(Occasion coordinate)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string loggedUser = HttpContext.Session.GetString("UserEmail");
                    coordinate.CoordinatorName = loggedUser;
                    dbContext.Occasions.Add(coordinate);
                    dbContext.SaveChanges();
                    return Redirect($"show/{coordinate.OccasionId}");
                }
                else
                {
                    User userinDb = dbContext.Users.Include(u => u.PlannedOccasion).ThenInclude(o=>o.Coordinator).FirstOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));
                    ViewBag.User = userinDb;
                    return View("New");
                }
            }
        }

        [HttpGet("show/{occasionId}")]
        public IActionResult Show(int occasionId)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                User userinDb = dbContext.Users.Include(u => u.PlannedOccasion).FirstOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userinDb;

                Occasion game = dbContext.Occasions.Include(o => o.Participants).ThenInclude(a => a.Participant).Include(o => o.Coordinator).FirstOrDefault(o => o.OccasionId == occasionId);

                return View(game);
            }

        }
        [HttpGet("rsvp/{occasionId}/{userId}/{status}")]
        public IActionResult Join(int occasionId, int userId, string status)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                if (status == "add")
                {
                    Attend response = new Attend();
                    response.UserId = userId;
                    response.OccasionId = occasionId;
                    dbContext.Attends.Add(response);
                    dbContext.SaveChanges();
                }
                if (status == "remove")
                {
                    Attend remove = dbContext.Attends.FirstOrDefault(a => a.OccasionId == occasionId && a.UserId == userId);
                    dbContext.Attends.Remove(remove);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet("destroy/{occasionId}")]
        public IActionResult Destroy(int occasionId)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                Occasion remove = dbContext.Occasions.FirstOrDefault(o => o.OccasionId == occasionId);
                dbContext.Occasions.Remove(remove);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
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
    }
}
