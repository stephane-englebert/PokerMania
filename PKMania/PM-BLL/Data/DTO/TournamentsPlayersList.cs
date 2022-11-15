
using PM_BLL.Data.DTO.Entities;

namespace PM_BLL.Data.DTO
{
    public class TournamentsPlayersList
    {
        public int TournamentId { get; set; }
        public IEnumerable<PlayerDTO> PlayersList { get; set; }
        public TournamentsPlayersList()
        {
            TournamentId = 0;
            PlayersList = new List<PlayerDTO>();
        }
    }
}
