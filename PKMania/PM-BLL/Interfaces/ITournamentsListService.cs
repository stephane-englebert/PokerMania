using PM_BLL.Data.DTO.Entities;

namespace PM_BLL.Interfaces
{
    public interface ITournamentsListService
    {
        TournamentsListDTO GetActiveTournaments();
        TournamentsListDTO GetPlayerActiveTournaments(int playerId);
        Boolean IsThereOngoingTrForOnePlayer(int playerId);
    }
}
