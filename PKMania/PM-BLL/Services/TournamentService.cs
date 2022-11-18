
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_DAL.Services;
using PM_DAL.Interfaces;

namespace PM_BLL.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentsTypesService _tournamentsTypesService = new TournamentsTypesService();
        private readonly ITournamentRepository _tournamentRepository = new TournamentRepository();
        private readonly IRegistrationsRepository _registrationsRepository = new RegistrationsRepository();
        private readonly IHandsRepository _handsRepository = new HandsRepository();
        private readonly IMemberRepository _memberRepository = new MemberRepository();

        public TournamentService()
        {
        }
        public void CreateTournament(DateTime startDate, string name, int type)
        {
            IEnumerable<TournamentsTypesDTO> trType = _tournamentsTypesService.GetTournamentsTypesById(type).ToList();
            int buyIn = trType.Single().BuyIn;
            int maxPlayers = trType.Single().MaxPlayers;
            int prizePool = buyIn * maxPlayers;
            int gainsSharingNr = trType.Single().GainsSharingNr;
            try
            {
                _tournamentRepository.CreateTournament(startDate, name, type, prizePool, gainsSharingNr);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        public void DeleteTournament(int trId)
        {
            try
            {
                _registrationsRepository.DeleteRegistrationsByTournament(trId);
                _memberRepository.UpdateCurrentTournIdForOneTournament(trId);
                _handsRepository.DeleteHandsByTournament(trId);
                _tournamentRepository.DeleteTournament(trId);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
        }
    }
}
