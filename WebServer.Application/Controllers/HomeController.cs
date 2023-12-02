using WebServer.Server.Controllers;
using WebServer.Server.HTTP;
using Sms.Data.Common;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository repo;
        public HomeController(
            Request request,Repository repo) 
            : base(request)
        {
            this.repo = repo;
        }

        public Response Index()
        {
            if(User.IsAuthenticated)
            {
                var model = repo.GetUserInfo(User.Id);
                return View(new {IsAuthenticated=true, AccountInfo =true,Username=model.Username,Email=model.Email});
            }
            return View(new { IsAuthenticated = false},"/Home/Index");
        }
        public Response ConfirmEmail(string userId)
        {
            repo.ConfirmEmail(userId);
            return Redirect("/");
        }
    }
}