using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BrightIdeas.Controllers
{
    public class IdeaController : Controller
    {
        private BrightIdeasContext _context;
 
        public IdeaController(BrightIdeasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("AllIdeas")]
        public IActionResult AllIdeas(){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            User currentUser = GetUser();
            ViewBag.currentUser = currentUser;
            ViewBag.allIdeas = GetAllIdeas();
            ViewBag.postError = HttpContext.Session.GetString("postError");
            HttpContext.Session.SetString("postError", "");
            return View("AllIdeas");
        }
        [HttpPost]
        [Route("newIdea")]
        public IActionResult NewIdea(string newContent, int posterID){
            if(newContent == null || newContent.Length < 10){
                HttpContext.Session.SetString("postError", "Ideas must be at least 10 characters long.");
                return RedirectToAction("AllIdeas");
            }
            else{
                System.Console.WriteLine("*************************************");
                System.Console.WriteLine(posterID);
                Idea newOne = new Idea{
                    Content = newContent,
                    UserID = posterID
                };
                _context.Ideas.Add(newOne);
                _context.SaveChanges();
                return RedirectToAction("AllIdeas");
            }
        }
        [HttpGet]
        [Route("like/{IdeaID}")]
        public IActionResult NewLike(int IdeaID){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            User currentUser = GetUser();
            Like newLike = new Like{
                UserID = currentUser.UserID,
                IdeaID = IdeaID
            };
            _context.Likes.Add(newLike);
            Idea currentIdea = _context.Ideas.Single(i => i.IdeaID == IdeaID);
            currentIdea.LikeCount = currentIdea.LikeCount+1;
            _context.SaveChanges();
            return RedirectToAction("AllIdeas");

        }
        [HttpGet]
        [Route("unlike/{IdeaID}")]
        public IActionResult UnLike(int IdeaID){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            User currentUser = GetUser();
            Idea currentIdea = _context.Ideas.Single(i => i.IdeaID == IdeaID);
            currentIdea.LikeCount = currentIdea.LikeCount-1;
            Like thisLike = _context.Likes.Single(i => i.UserID == currentUser.UserID && i.IdeaID == currentIdea.IdeaID);
            _context.Likes.Remove(thisLike);
            _context.SaveChanges();
            return RedirectToAction("AllIdeas");

        }
        [HttpGet]
        [Route("allLikes/{IdeaID}")]
        public IActionResult AllLikes(int IdeaID){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            ViewBag.currentIdea = _context.Ideas.Include(l => l.Likes).ThenInclude(u => u.User).Include(p => p.User).Single(n => n.IdeaID == IdeaID);
            return View("AllLikes");
        }
        [HttpGet]
        [Route("personView/{UserID}")]
        public IActionResult PersonView(int UserID){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            User personViewed = _context.Users.Single(i => i.UserID == UserID);
            ViewBag.personViewed = personViewed;
            bool likesExist = _context.Likes.Any(u => u.UserID == personViewed.UserID);
            if (likesExist == true){
                List<Like> thisPersonLikes = _context.Likes.Where(i => i.UserID == personViewed.UserID).ToList();
                ViewBag.postsLiked = thisPersonLikes.Count;
            }
            else{
                ViewBag.postsLiked = 0;
            }
            bool postsExist = _context.Ideas.Any(u => u.UserID == personViewed.UserID);
            if (postsExist == true){
                List<Idea> thisPersonPosts = _context.Ideas.Where(i => i.UserID == personViewed.UserID).ToList();
                ViewBag.posts = thisPersonPosts.Count;
            }
            else{
                ViewBag.posts = 0;
            }
            return View("PersonView");
        }
        [HttpGet]
        [Route("deleteIdea/{IdeaID}")]
        public IActionResult Delete(int IdeaID){
            if(LoggedIn() == false){
                return RedirectToAction("Index", "User");
            }
            Idea currentIdea = _context.Ideas.Single(i => i.IdeaID == IdeaID);
            _context.Ideas.Remove(currentIdea);
            _context.SaveChanges();
            return RedirectToAction("AllIdeas");
        }




        public bool LoggedIn(){
            int? loggedIn = HttpContext.Session.GetInt32("userID");
            if(loggedIn == null){
                return false;
            }
            else{
                return true;
            }
        }
        public User GetUser(){
            bool likesExist = _context.Likes.Any(u => u.UserID == HttpContext.Session.GetInt32("userID"));
            if(likesExist == true){
                User currentUser = _context.Users.Include(l => l.Likes).ThenInclude(i => i.Idea).Single(p => p.UserID == HttpContext.Session.GetInt32("userID"));
                return currentUser;
            }
            else{
            User currentUser = _context.Users.Single(p => p.UserID == HttpContext.Session.GetInt32("userID"));
            return currentUser;
            }
        }
        public List<Idea> GetAllIdeas(){
            List<Idea> All = _context.Ideas.Include(j => j.User).Include(i => i.Likes).ThenInclude(u => u.User).OrderByDescending(l => l.LikeCount).ToList();
            return All;
        }

    }
}