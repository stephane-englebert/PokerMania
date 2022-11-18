
using System.ComponentModel.DataAnnotations;

namespace PM_BLL.Data.DTO.Forms
{
    public class RegisterToTournamentFormDTO
    {
        [Required]
        public int TournamentId { get; set; }
    }
}
