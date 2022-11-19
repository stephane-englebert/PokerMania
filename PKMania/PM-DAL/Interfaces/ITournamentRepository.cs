
namespace PM_DAL.Interfaces
{
    public interface ITournamentRepository
    {
        void CreateTournament(DateTime startDate, string name, int type, int prizePool, int gainsSharingNr);
        void DeleteTournament(int trId);
        void UpdateNumberPlayersRegistered(int trId, int incOrDec);
        Boolean CanJoinLobby(int trId, int playerId);
        string GetTournamentStatus(int trId);
        void SetTournamentStatus(int trId, string status);
        void StartTournament(int trId);
    }
}
