using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;
using System.Linq;

namespace BrightIdeas.Controllers
{
    public class UserController : Controller
    {
        private BrightIdeasContext _context;
 
        public UserController(BrightIdeasContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.existsError = HttpContext.Session.GetString("existsError");
            ViewBag.loginError = HttpContext.Session.GetString("loginError");
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model){
            if(!ModelState.IsValid){
                return View("Index");
            }
            else{
                bool exists = _context.Users.Any(u => u.Email == model.Email);
                if (exists == true){
                    HttpContext.Session.SetString("existsError", "That email address is already in use, please try again.");
                    return RedirectToAction ("Index");
                }
                else
                {
                    User newUser = new User{
                        Name = model.Name,
                        Email = model.Email,
                        Alias = model.Alias,
                        Password = model.Password,
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    User currentUser = _context.Users.Single(u => u.Email == newUser.Email);
                    HttpContext.Session.SetInt32("userID", currentUser.UserID);
                    return RedirectToAction("AllIdeas", "Idea");
                } 
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password){
            if (Email == null || Password == null){
                HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                return RedirectToAction("Index");
            }
            else{
                bool exists = _context.Users.Any(u => u.Email == Email);
                if(exists == true){
                    User currentUser = _context.Users.Single(u => u.Email == Email);
                    if (currentUser.Password == Password){
                        HttpContext.Session.SetInt32("userID", currentUser.UserID);
                        return RedirectToAction("AllIdeas", "Idea");
                    }
                    else{
                        HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                        return RedirectToAction("Index");
                    }
                }
                else{
                    HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }


    }
}