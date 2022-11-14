
using PM_DAL.Data.Entities;

namespace PM_BLL.Data.DTO.Entities
{
    public class TournamentsTypesDTO
    {
        public int Id { get; set; }
        public int BuyIn { get; set; }
        public int LateRegistrationLevel { get; set; }
        public int StartingStack { get; set; }
        public Boolean Rebuy { get; set; }
        public int RebuyLevel { get; set; }
        public int LevelsDuration { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int PlayersPerTable { get; set; }
        public int MaxPaidPlaces { get; set; }
        public int GainsSharingNr { get; set; }
        public TournamentsTypesDTO(TournamentsTypes tt)
        {
            Id = tt.Id;
            BuyIn = tt.BuyIn;
            LateRegistrationLevel = tt.LateRegistrationLevel;
            StartingStack = tt.StartingStack;
            Rebuy = tt.Rebuy;
            RebuyLevel = tt.RebuyLevel;
            LevelsDuration = tt.LevelsDuration;
            MinPlayers = tt.MinPlayers;
            MaxPlayers = tt.MaxPlayers;
            PlayersPerTable = tt.PlayersPerTable;
            MaxPaidPlaces = tt.MaxPaidPlaces;
            GainsSharingNr = tt.GainsSharingNr;
        }
    }
}
