
namespace PM_BLL.Data.DTO.Entities
{
    public class RankedPlayerDTO
    {
        public int Rank { get; set; }
        public int TableNr { get; set; }
        public int PlayerId { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        public int Stack { get; set; }
        public Boolean Eliminated { get; set; }
        public RankedPlayerDTO()
        {
            Rank = 0;
            TableNr = 0;
            PlayerId = 0;
            Pseudo = "";
            Stack = 0;
            Eliminated = false;
        }
    }
}
