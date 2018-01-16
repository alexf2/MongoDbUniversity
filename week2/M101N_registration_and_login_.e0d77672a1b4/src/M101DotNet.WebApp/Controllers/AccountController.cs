﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Account;
using MongoDB.Bson;

namespace M101DotNet.WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Email = model.Email?.Trim();

            var blogContext = new BlogContext();            
            var bld = Builders<User>.Filter;

            var user = await blogContext.Users.Find(bld.Regex(u => u.Email, new BsonRegularExpression($"^{model.Email}$", "i"))).FirstOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email address has not been registered.");
                return View(model);
            }

            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                },
                "ApplicationCookie");

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignIn(identity);

            return Redirect(GetRedirectUrl(model.ReturnUrl));
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Name = model.Name?.Trim();
            model.Email = model.Email?.Trim();

            var blogContext = new BlogContext();            

            var userExisting = await blogContext.Users.Find(u => u.Name.ToLower() == model.Name.ToLower()).FirstOrDefaultAsync();
            if (userExisting != null)
            {
                ModelState.AddModelError("Name", $"User '{model.Name}' already exists");
                return View(model);
            }

            userExisting = await blogContext.Users.Find(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefaultAsync();
            if (userExisting != null)
            {
                ModelState.AddModelError("Email", $"User with Email '{model.Email}' already exists");
                return View(model);
            }

            var user = new User() {Name = model.Name, Email = model.Email};
            await blogContext.Users.InsertOneAsync(user);

            return RedirectToAction("Index", "Home");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
    }
}