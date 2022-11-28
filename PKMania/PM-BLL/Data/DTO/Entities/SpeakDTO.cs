
namespace PM_BLL.Data.DTO.Entities
{
    public class SpeakDTO
    {
        public int IdTr { get; set; }
        public Guid Guid { get; set; }
        public int TurnSpeak { get; set; }
        public int IdPlay { get; set; }
        public int IdNext { get; set; }
        public string Choice { get; set; } = string.Empty;
        public int NewPlayerBet { get; set; }
        public SpeakDTO(Guid guid)
        {
            IdTr = 0;
            Guid = guid;
            TurnSpeak = 0;
            IdPlay = 0;
            IdNext = 0;
            Choice = "";
            NewPlayerBet = 0;
        }
    }
}
