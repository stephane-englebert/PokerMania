using System.ComponentModel.DataAnnotations;

namespace PM_BLL.Data.DTO.Forms
{
    public class MemberLoginFormDTO
    {
        [Required]
        public string UserIdentifier { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
