using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface ITournamentsListRepository
    {
        IEnumerable<Tournament> GetActiveTournaments(IEnumerable<Gains> allGains);
    }
}
