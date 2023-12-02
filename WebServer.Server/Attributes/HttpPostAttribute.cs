using WebServer.Server.HTTP;

namespace WebServer.Server.Attributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute() : base(Method.Post)
        {
        }
    }
}
