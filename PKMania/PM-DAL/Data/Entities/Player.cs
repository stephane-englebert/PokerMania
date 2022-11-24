
namespace PM_DAL.Data.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        public int TableNr { get; set; }
        public int Stack { get; set; }
        public int BonusTime { get; set; }
        public DateTime EliminatedAt { get; set; }
        public Boolean Disconnected { get; set; }
    }
}
