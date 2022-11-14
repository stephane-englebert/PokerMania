using Microsoft.AspNetCore.SignalR;
using PM_Backend.Services;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_BLL.Services;

namespace PM_Backend.Hubs
{
    public class PkHub : Hub
    {
        //private readonly ITournamentsListService _tournamentsListService;
        //private TournamentsListDTO _tournamentsList;
        private int testInt;

        public PkHub()
        {
            //_tournamentsListService = tournamentsListService;
            //this._tournamentsList = _tournamentsListService.GetActiveTournaments();
            this.testInt = 0;
        }

        public void SendMsgToAll(string message)
        {
           Console.WriteLine(message); 
           if(Clients != null)
            {
                Clients.All.SendAsync("msgToAll",message);
            }
        }
        public void TestMsg(string msg)
        {
            testInt++;
            this.SendMsgToAll("Votre message: " + msg + " [testInt = " + testInt + "]");
            Clients.All.SendAsync("inc",testInt);
        }
        public void GetInfosActivTournaments(TournamentsListService tournamentsListService)
        {
            try
            {
                TournamentsListDTO tournamentsList = tournamentsListService.GetActiveTournaments();
                Clients.Caller.SendAsync("sendInfosActivTournaments", tournamentsList);
                Console.WriteLine("tesssssst");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
