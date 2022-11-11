
namespace PM_BLL.Data.DTO.Entities
{
    public class TournamentsListDTO
    {
        public IEnumerable<TournamentDTO>? Tournaments { get; set; }
        public IEnumerable<bool>? CanJoinTournaments { get; set; }
        public TournamentsListDTO(IEnumerable<TournamentDTO>? tournaments, IEnumerable<bool>? canJoinTournaments)
        {
            Tournaments = tournaments;
            CanJoinTournaments = canJoinTournaments;
        }
    }
}
