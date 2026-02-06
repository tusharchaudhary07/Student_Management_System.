using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyLoginPage.Models;

namespace MyLoginPage.Controllers
{
    public class AccountController : Controller
    {

        // GET: Account
        UserDbContext usdb = new UserDbContext();

        [HttpGet]
        public ActionResult Login()
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetNoStore();


            return View();
        }

        [HttpPost]
        [Obsolete]
        public ActionResult Login(UserClass user)
        {
            if (ModelState.IsValid)
            {
                
                // Hardcoded Admin Login
                if (user.USERID == "admin" && user.PASSWORD == "adminabc")
                {
                    Session["USERID"] = "admin";
                    Session["ROLE"] = "admin";
                    return RedirectToAction("AdminDashboard", "Admin");
                }

                // DB Login
                var validatedUser = usdb.ValidateUser(user.USERID, user.PASSWORD);

                if (validatedUser != null)
                {
                    Session["USERID"] = validatedUser.USERID;
                    Session["ROLE"] = validatedUser.ROLE;

                    if (validatedUser.ROLE == "admin")
                        return RedirectToAction("AdminDashboard", "Admin");
                    else
                        return RedirectToAction("SubmitDetails", "Student");
                }

                ViewBag.Message = "Invalid username or password.";
                return View(user);
            }

            ViewBag.Message = "Please fill in all fields correctly.";
            return View(user);
        }



        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Obsolete]
        public ActionResult Register(UserClass user)
        {
            if (usdb.RegisterUser(user))
            {
                ViewBag.Message = "Registration Successfull, Please Log in";
                return RedirectToAction("Login", "Account");
            }

            else
            {
                ViewBag.Message = "User already exist";
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        public ActionResult Show()
        {
            return View();
        }
    }
}