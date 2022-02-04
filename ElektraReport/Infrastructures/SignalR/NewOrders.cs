using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Infrastructures.SignalR
{
    public class NewOrders : Hub
    {
        [HubMethodName("SendNewOrders")]
        public void SendNewOrders(Guid CompanyId)
        {
          //  messageDto.ConnectionId = Context.ConnectionId;
            Clients.All.SendAsync("CompanyId", CompanyId);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
