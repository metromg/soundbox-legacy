namespace Soundbox.Reloaded.Ui.Web.Controllers.Hubs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Microsoft.AspNet.SignalR;

    public class SoundHub : Hub
    {
        public static List<string> Users = new List<string>();
            
        public void Send(int count)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SoundHub>();
            context.Clients.All.updateUsersOnlineCount(count);
        }
            
        public override Task OnConnected()
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }

            Send(Users.Count);

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            string clientId = GetClientId();
            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }

            Send(Users.Count);

            return base.OnReconnected();
        }
            
        public override Task OnDisconnected(bool stopCalled)
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) > -1)
            {
                Users.Remove(clientId);
            }

            Send(Users.Count);

            return base.OnDisconnected(stopCalled);
        }
            
        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                // clientId passed from application 
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }
    }
}