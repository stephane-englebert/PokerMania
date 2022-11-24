
using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface ITournamentRepository
    {
        int CreateTournament(DateTime startDate, string name, int type, int prizePool, int gainsSharingNr);
        void DeleteTournament(int trId);
        void UpdateNumberPlayersRegistered(int trId, int incOrDec);
        Boolean CanJoinLobby(int trId, int playerId);
        string GetTournamentStatus(int trId);
        void SetTournamentStatus(int trId, string status);
        void LaunchTournament(int trId);
        void StartTournament(int trId);
        IEnumerable<Clean> GetIdTournamentsToClean();
    }
}
