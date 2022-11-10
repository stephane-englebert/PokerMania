using Microsoft.AspNetCore.SignalR;

namespace PM_Backend.Hubs
{
    public class PkHub : Hub
    {
        public void SendMsgToAll(string message)
        {
           Console.WriteLine(message); 
           if(Clients != null)
            {
                Clients.All.SendAsync("msgToAll",message);
            }
        }
    }
}
