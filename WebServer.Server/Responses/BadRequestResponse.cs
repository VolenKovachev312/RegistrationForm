using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class BadRequestResponse : Response
    {
        public BadRequestResponse()
             : base(StatusCode.BadRequest)
        {
        }
    }
}
