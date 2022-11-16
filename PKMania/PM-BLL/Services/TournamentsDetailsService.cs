
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Interfaces;
using PM_DAL.Services;

namespace PM_BLL.Services
{
    public class TournamentsDetailsService : ITournamentsDetailsService
    {
        private readonly ITournamentsListRepository _tournamentsListRepository = new TournamentsListRepository();

        public TournamentsDetailsService()
        {

        }

        public IEnumerable<TournamentDetailsDTO> GetTournamentsDetails(TournamentsListDTO trList,IEnumerable<TournamentsTypesDTO> trTypes)
        {
            if (trList.Tournaments != null)
            {
                foreach (TournamentDTO dto in trList.Tournaments)
                {
                    TournamentsTypesDTO tType = trTypes.Single(ty => ty.Id == dto.TournamentType);
                    TournamentDetailsDTO tDetails = new TournamentDetailsDTO(dto, tType);
                    yield return tDetails;
                }
            }
            
        }
    }
}
