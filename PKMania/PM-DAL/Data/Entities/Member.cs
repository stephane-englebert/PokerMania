using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_DAL.Data.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Bankroll { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDisconnected { get; set; }
        public int CurrentTournament { get; set; }
    }
}