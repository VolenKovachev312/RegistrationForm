namespace SMS
{
    using WebServer.Server;
    using WebServer.Server.Routing;
    using Sms.Data.Common;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static async Task Main()
        {
            var server = new HttpServer(routes => routes
               .MapControllers()
               .MapStaticFiles());

            server.ServiceCollection
                .Add<Repository>();

            await server.Start();
        }
    }
}