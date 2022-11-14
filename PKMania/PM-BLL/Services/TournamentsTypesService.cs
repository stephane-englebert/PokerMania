using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Interfaces;

namespace PM_BLL.Services
{
    public class TournamentsTypesService : ITournamentsTypesService
    {
        private readonly ITournamentsTypesRepository _tournamentsTypesRepository;
        public TournamentsTypesService(ITournamentsTypesRepository tournamentsTypesRepository)
        {
            _tournamentsTypesRepository = tournamentsTypesRepository;
        }
        public IEnumerable<TournamentsTypesDTO> GetAllTournamentsTypes()
        {
            return _tournamentsTypesRepository.GetAllTournamentsTypes().Select(t => new TournamentsTypesDTO(t));
        }
    }
}
