
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Interfaces;
using PM_DAL.Services;

namespace PM_BLL.Services
{
    public class TournamentsListService: ITournamentsListService
    {
        private readonly ITournamentsListRepository _tournamentsListRepository;
        public TournamentsListService(ITournamentsListRepository tournamentsListRepository)
        {
            _tournamentsListRepository = tournamentsListRepository;
        }
        public TournamentsListDTO GetActiveTournaments()
        {
            IEnumerable<TournamentDTO> tournList = _tournamentsListRepository.GetActiveTournaments().Select(t => new TournamentDTO(t));
            IEnumerable<bool> bools = new HashSet<bool>();
            TournamentsListDTO dto = new TournamentsListDTO(tournList,bools);
            return dto;
        }
    }
}
