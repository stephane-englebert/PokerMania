using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Interfaces;
using PM_DAL.Services;

namespace PM_BLL.Services
{
    public class TournamentsTypesService : ITournamentsTypesService
    {
        private readonly ITournamentsTypesRepository _tournamentsTypesRepository = new TournamentsTypesRepository();
        public TournamentsTypesService()
        {
        }
        public IEnumerable<TournamentsTypesDTO> GetAllTournamentsTypes()
        {
            return _tournamentsTypesRepository.GetAllTournamentsTypes().Select(t => new TournamentsTypesDTO(t));
        }
        public IEnumerable<TournamentsTypesDTO> GetTournamentsTypesById(int id)
        {
            return _tournamentsTypesRepository.GetTournamentsTypesById(id).Select(t => new TournamentsTypesDTO(t));
        }
    }
}
