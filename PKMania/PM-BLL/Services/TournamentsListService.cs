
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using PM_DAL.Services;

namespace PM_BLL.Services
{
    public class TournamentsListService: ITournamentsListService
    {
        private readonly ITournamentsListRepository _tournamentsListRepository;
        private readonly IGainsRepository _gainsRepository;
        public TournamentsListService(
            ITournamentsListRepository tournamentsListRepository,
            IGainsRepository gainsRepository
        ) {
            _tournamentsListRepository = tournamentsListRepository;
            _gainsRepository = gainsRepository;
        }
        public TournamentsListDTO GetActiveTournaments()
        {
            IEnumerable<Gains> allGains = _gainsRepository.GetAllGainsSharings();
            IEnumerable<TournamentDTO> tournList =
                _tournamentsListRepository
                    .GetActiveTournaments(allGains)
                    .Select(t => new TournamentDTO(t));
            List<TournamentDTO> trList = new List<TournamentDTO>();
            foreach(TournamentDTO tournament in tournList)
            {
                int gsn = tournament.GainsSharingNr;
                IEnumerable<Gains> trGains = allGains.Where(g => g.GainsSharingNr == gsn);
                List<Gains> newGains = new List<Gains>();
                int cpt = 0;
                int rpp = 0;
                int prizesCumulated = 0;
                foreach (Gains g in trGains)
                {
                    newGains.Add(g);
                    int range = g.EndPlace - g.StartPlace + 1;
                    rpp += range;
                    int prizePerRange = (int)((int)(tournament.PrizePool / 100) * (decimal)g.Percentage);
                    int prizePerPlayer = (int)prizePerRange / range;
                    newGains[cpt].NumberChips = prizePerPlayer;
                    prizesCumulated += prizePerPlayer * range;
                    cpt++;
                }
                int solde = tournament.PrizePool - prizesCumulated;
                newGains[0].NumberChips += solde;
                tournament.Gains = newGains;
                tournament.RealPaidPlaces = rpp;

                trList.Add(tournament);
            }
            IEnumerable<bool> bools = new HashSet<bool>();
            TournamentsListDTO dto = new TournamentsListDTO(trList,bools);
            return dto;
        }
        public TournamentsListDTO GetPlayerActiveTournaments(int playerId)
        {
            TournamentsListDTO allTourn = this.GetActiveTournaments();

            return allTourn;
        }
    }
}
