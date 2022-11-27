
namespace PM_BLL.Data.DTO.Entities
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        public int GeneralRanking { get; set; }
        public int TableRanking { get; set; }
        public Boolean Eliminated { get; set; }
        public int SittingAtTable { get; set; }
        public int SeatNr { get; set; }
        public int Stack { get; set; }
        public int MoneyInPot { get; set; }
        public Boolean Disconnected { get; set; }
        public Boolean TurnToPlay { get; set; }
        public IEnumerable<CardDTO> PrivateCards { get; set; }
        public int BonusTime { get; set; }
        public PlayerDTO()
        {
            Id = 0;
            Pseudo = "";
            GeneralRanking = 0;
            TableRanking = 0;
            Eliminated = false;
            SittingAtTable = 0;
            SeatNr = 0;
            Stack = 0;
            MoneyInPot = 0;
            Disconnected = true;
            TurnToPlay = false;
            PrivateCards = new List<CardDTO>();
            BonusTime = 120;
        }
    }
}
