using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SignalRServer.Controllers
{
    [Route("example")]
    public class ExampleController : Controller
    {
        private readonly ExampleHub _hub;

        public ExampleController(ExampleHub hub)
        {
            _hub = hub;
        }

        [HttpPost, Route("")]
        public async Task ExampleMethod() 
        {
            await _hub.SendGroupMessage("Server", "Hello, this an SignalR message!");
        }
    }
}