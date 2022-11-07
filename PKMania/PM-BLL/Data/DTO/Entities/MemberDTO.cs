using PM_DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Data.DTO.Entities
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Bankroll { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDisconnected { get; set; }
        public int CurrentTournament { get; set; }
        public MemberDTO(Member member)
        {
            Id = member.Id;
            Role = member.Role;
            Pseudo = member.Pseudo;
            Email = member.Email;
            Bankroll = member.Bankroll;
            IsPlaying = member.IsPlaying;
            IsDisconnected = member.IsDisconnected;
            CurrentTournament = member.CurrentTournament;
        }
    }
}
