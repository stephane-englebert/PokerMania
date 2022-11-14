using PM_BLL.Data.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Interfaces
{
    public interface ITournamentsTypesService
    {
        IEnumerable<TournamentsTypesDTO> GetAllTournamentsTypes();
    }
}
