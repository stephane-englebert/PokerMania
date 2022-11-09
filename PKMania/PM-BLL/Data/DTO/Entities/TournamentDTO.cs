using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public GainsDTO[]? Gains { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentSmallBlind { get; set; }
        public int CurrentBigBlind { get; set; }
        public int CurrentAnte { get; set; }
        public int NextLevel { get; set; }
        public int TimeBeforeNextLevel { get; set; }
        public int NextSmallBlind { get; set; }
        public int NextBigBlind { get; set; }
        public int NextAnte { get; set; }

    }
}
