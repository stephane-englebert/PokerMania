using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Data.DTO.Forms
{
    public class MemberRegisterFormDTO
    {
        [Required]
        public string Pseudo { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
