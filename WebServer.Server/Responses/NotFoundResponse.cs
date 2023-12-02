using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse()
             : base(StatusCode.NotFound)
        {
        }
    }
}
