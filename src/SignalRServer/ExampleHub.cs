using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer
{
    public class ExampleHub : Hub
    {
        private readonly IHubContext<ExampleHub> _context;
        private static string STATIC_GROUP = "staticGroup";

        public ExampleHub(IHubContext<ExampleHub> context)
        {
            _context = context;
        }

        public async Task Subscribe() 
        {
            await _context.Groups.AddToGroupAsync(Context.ConnectionId, STATIC_GROUP);
        }

        public async Task SendGroupMessage(string user, string message) 
        {
            await _context.Clients.Groups(STATIC_GROUP).SendAsync("ReceiveMessage", user, message);
        }
    }
}