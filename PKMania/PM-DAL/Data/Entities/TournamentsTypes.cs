
namespace PM_DAL.Data.Entities
{
    public class TournamentsTypes
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
    }
}
