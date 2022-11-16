
namespace PM_BLL.Data.DTO.Entities
{
    public class TournamentPlayersDTO
    {
        public int TournamentId { get; set; }
        public IEnumerable<PlayerDTO> Players { get; set; }
        public IEnumerable<RankedPlayerDTO> RankedPlayers { get; set; }
        public TournamentPlayersDTO()
        {
            TournamentId = 0;
            Players = new List<PlayerDTO>();
            RankedPlayers = new List<RankedPlayerDTO>();
        }
        public TournamentPlayersDTO(int tournamentId, IEnumerable<PlayerDTO> players, IEnumerable<RankedPlayerDTO> rankedPlayers)
        {
            TournamentId = tournamentId;
            Players = players;
            RankedPlayers = rankedPlayers;
        }
    }
}
