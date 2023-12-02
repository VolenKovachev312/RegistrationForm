using WebServer.Server.Attributes;
using WebServer.Server.Controllers;
using WebServer.Server.HTTP;
using Sms.Data.Common;
using SMS.Models;
using System;

namespace SMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly Repository repo;

        public UsersController(
            Request request,
            Repository repo) 
            : base(request)
        {
            this.repo = repo;
        }

        public Response Login()
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View(new { IsAuthenticated = false });
        }

        [HttpPost]
        public Response Login(LoginViewModel model)
        {
            Request.Session.Clear();
            string? id = repo.Login(model);

            if (id == null)
            {
                return View(new { ErrorMessage = "Incorect Login" }, "/Error");
            }

            SignIn(id);

            CookieCollection cookies = new CookieCollection();
            cookies.Add(Session.SessionCookieName,
                Request.Session.Id);

            return Redirect("/");
        }

        
        [HttpPost]
        public Response ChangeAccountInfo(AccountInfoViewModel model)
        {
            try
            {
                repo.ChangeUserInfo(User.Id, model);

            }
            catch(Exception e)
            {
                return View(new { ErrorMessage = e.Message }, "/Error");
            }
            return Redirect("/");
        }

        public Response DeleteUser()
        {
            repo.DeleteUser(User.Id);
            SignOut();
            return View(new { IsAuthenticated = false }, "/Home/Index");
        }

        public Response Register()
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View(new { IsAuthenticated = false });
        }

        [HttpPost]
        public Response Register(RegisterViewModel model)
        {
            var (isRegistered, error) = repo.Register(model);

            if (isRegistered)
            {
                return Redirect("/Users/Login");
            }

            return View(new { ErrorMessage = error }, "/Error");
        }

        [Authorize]
        public Response Logout()
        {
            SignOut();

            return Redirect("/");
        }
    }
}
