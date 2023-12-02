using WebServer.Server.HTTP;
using System;

namespace WebServer.Server.Routing
{
    public interface IRoutingTable
    {
        IRoutingTable Map(Method method, string path, Func<Request, Response> responseFunction);
    }
}
