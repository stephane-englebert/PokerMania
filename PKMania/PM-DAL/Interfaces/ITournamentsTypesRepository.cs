
using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface ITournamentsTypesRepository
    {
        IEnumerable<TournamentsTypes> GetAllTournamentsTypes();
        IEnumerable<TournamentsTypes> GetTournamentsTypesById(int id);
    }
}
