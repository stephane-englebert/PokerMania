
using PM_BLL.Data.DTO.Entities;

namespace PM_BLL.Interfaces
{
    public interface ITournamentsDetailsService
    {
        IEnumerable<TournamentDetailsDTO> GetTournamentsDetails(TournamentsListDTO trList, TournamentsTypesDTO[] trTypes);

    }
}
