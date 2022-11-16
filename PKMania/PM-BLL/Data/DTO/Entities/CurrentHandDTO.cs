
namespace PM_BLL.Data.DTO.Entities
{
    public class CurrentHandDTO
    {
        public int TournamentId { get; set; }
        public int TableNr { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public int Pot { get; set; }
        public IEnumerable<PlayerDTO> Players { get; set; }
        public int SeatNrPlayerToPlay { get; set; }
        public int SeatNrButton { get; set; }
        public int SeatNrSmallBlind { get; set; }
        public int SeatNrBigBlind { get; set; }
        public IEnumerable<CardDTO> Flop { get; set; }
        public CardDTO Turn { get; set; }
        public CardDTO River { get; set; }
        public string HandHistory { get; set; } = string.Empty;
        public CurrentHandDTO()
        {
            TournamentId = 0;
            TableNr = 0;
            StartedOn = DateTime.Now;
            FinishedOn = new DateTime(1970,1,1,0,0,0);
            Pot = 0;
            Players = new List<PlayerDTO>();
            SeatNrPlayerToPlay = 0;
            SeatNrButton = 0;
            SeatNrSmallBlind = 0;
            SeatNrBigBlind = 0;
            Flop = new List<CardDTO>();
            Turn = new CardDTO();
            River = new CardDTO();
            HandHistory = "";
        }
    }
}
