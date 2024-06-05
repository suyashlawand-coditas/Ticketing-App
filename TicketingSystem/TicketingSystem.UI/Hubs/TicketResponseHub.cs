using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.Hubs
{
    public sealed class TicketResponseHub : Hub
    {

        public readonly IJwtService _jwtService;

        public TicketResponseHub(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ConnectionInfo", $"{Context.ConnectionId} has joined");
        }

        public async Task JoinGroup(string name)
        {
            string? jwtToken = Context.GetHttpContext()!.Request.Cookies["Authorization"];
            if (String.IsNullOrWhiteSpace(jwtToken))
            {
                return;
            }
            else
            {
                _jwtService.VerifyJwtToken(jwtToken);
                await Groups.AddToGroupAsync(Context.ConnectionId, name);
                await Clients.Group(name).SendAsync("ReceiveGroupMessage", $"{Context.ConnectionId} has joined the group!");
            }
        }

    }
}
