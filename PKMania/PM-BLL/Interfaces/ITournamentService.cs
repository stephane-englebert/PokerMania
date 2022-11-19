
namespace PM_BLL.Interfaces
{
    public interface ITournamentService
    {
        void CreateTournament(DateTime startDate, string name, int type);
        void DeleteTournament(int trId);
        Boolean CanJoinLobby(int tr, int id);
        Boolean StartTournament(int trId);
        void CloseTournament(int trId);
        void PlayerIsJoiningLobby(int trId, int playerId);
    }
}
