using WebServer.Server.HTTP;

namespace WebServer.Server.Attributes
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute() : base(Method.Get)
        {
        }
    }
}
