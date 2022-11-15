
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.Mappers;
using PM_BLL.Interfaces;
using PM_DAL.Interfaces;

namespace PM_BLL.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly IRegistrationsRepository _registrationsRepository;
        public RegistrationsService(IRegistrationsRepository registrationsRepository)
        {
            _registrationsRepository = registrationsRepository;
        }
        public IEnumerable<PlayerDTO> GetAllRegistrationsForOneTournament(int trId)
        {
            return  _registrationsRepository.GetAllRegistrationsForOneTournament(trId).Select(r => r.PlayerDalToDTO());
        }
    }
}
