
using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface IRegistrationsRepository
    {
        IEnumerable<Player> GetAllRegistrationsForOneTournament(int trId);
        Boolean IsPlayerRegistered(int trId, int playerId);
        void UnregisterTournament(int trId, int playerId);
        void RegisterTournament(int trId, int playerId);
        void DeleteRegistrationsByTournament(int trId);
        Boolean StillFreePlacesForTournament(int trId);
        void EliminateFromTournament(int trId, int playerId);
    }
}
