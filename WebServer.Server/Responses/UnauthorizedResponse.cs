using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class UnauthorizedResponse : Response
    {
        public UnauthorizedResponse()
             : base(StatusCode.Unauthorized)
        {
        }
    }
}
