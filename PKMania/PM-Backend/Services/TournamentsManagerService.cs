using PM_Backend.Hubs;

namespace PM_Backend.Services
{
    public class TournamentsManagerService: ITournamentsManagerService
    {
        private readonly PkHub _hub;
        public TournamentsManagerService(PkHub hub)
        {
            _hub = hub;
        }
        public void TournamentsManager(){
            _hub.SendMsgToAll("Petit coucou à tous les joueurs!");
        }
    }
}
