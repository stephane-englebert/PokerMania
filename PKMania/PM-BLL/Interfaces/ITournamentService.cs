
namespace PM_BLL.Interfaces
{
    public interface ITournamentService
    {
        void CreateTournament(DateTime startDate, string name, int type);
        void DeleteTournament(int trId);
    }
}
