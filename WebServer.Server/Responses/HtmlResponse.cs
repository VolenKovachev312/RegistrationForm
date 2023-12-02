using System;
using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string text)
            : base(text, ContentType.Html)
        {
        }
    }
}
