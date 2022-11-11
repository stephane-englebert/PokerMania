
using PM_DAL.Data.Entities;

namespace PM_BLL.Data.DTO.Entities
{
    public class TournamentDTO
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TournamentType { get; set; }
        public int RegistrationsNumber { get; set; }
        public int RealPaidPlaces { get; set; }
        public IEnumerable<Gains>? Gains { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentSmallBlind { get; set; }
        public int CurrentBigBlind { get; set; }
        public int CurrentAnte { get; set; }
        public int NextLevel { get; set; }
        public int TimeBeforeNextLevel { get; set; }
        public int NextSmallBlind { get; set; }
        public int NextBigBlind { get; set; }
        public int NextAnte { get; set; }
        public TournamentDTO(Tournament t)
        {
            Id = t.Id;
            Status = t.Status;
            StartedOn = t.StartedOn;
            FinishedOn = t.FinishedOn;
            Name = t.Name;
            TournamentType = t.TournamentType;
            RegistrationsNumber = t.RegistrationsNumber;
            RealPaidPlaces = t.RealPaidPlaces;
            Gains = t.Gains;
            CurrentLevel = t.CurrentLevel;
            CurrentSmallBlind = t.CurrentSmallBlind;
            CurrentBigBlind = t.CurrentBigBlind;
            CurrentAnte = t.CurrentAnte;
            NextLevel = t.NextLevel;
            TimeBeforeNextLevel = t.TimeBeforeNextLevel;
            NextSmallBlind = t.NextSmallBlind;
            NextBigBlind = t.NextBigBlind;
            NextAnte = t.NextAnte;
        }
    }
}
