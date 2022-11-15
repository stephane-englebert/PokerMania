
namespace PM_BLL.Data.DTO.Entities
{
    public class TournamentDetailsDTO
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int BuyIn { get; set; }
        public Boolean Rebuy { get; set; }
        public int RebuyLevel { get; set; }
        public int PrizePool { get; set; }
        public int PlayersPerTable { get; set; }
        public int MaxPaidPlaces { get; set; }
        public int RegistrationsNumber { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int StartingStack { get; set; }
        public int LevelsDuration { get; set; }
        public int GainsSharingNr { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public int TournamentType { get; set; }
        public int RealPaidPlaces { get; set; }
        public TournamentDetailsDTO(TournamentDTO t,TournamentsTypesDTO tt)
        {
            Id = t.Id;
            Status = t.Status;
            Name = t.Name;
            BuyIn = tt.BuyIn;
            Rebuy = tt.Rebuy;
            RebuyLevel = tt.RebuyLevel;
            PrizePool = t.PrizePool;
            PlayersPerTable = tt.PlayersPerTable;
            MaxPaidPlaces = tt.MaxPaidPlaces;
            RegistrationsNumber = t.RegistrationsNumber;
            MinPlayers = tt.MinPlayers;
            MaxPlayers = tt.MaxPlayers;
            StartingStack = tt.StartingStack;
            LevelsDuration = tt.LevelsDuration;
            GainsSharingNr = t.GainsSharingNr;
            StartedOn = t.StartedOn;
            FinishedOn = t.FinishedOn;
            TournamentType = t.TournamentType;
            RealPaidPlaces = t.RealPaidPlaces;
        }
    }
}
