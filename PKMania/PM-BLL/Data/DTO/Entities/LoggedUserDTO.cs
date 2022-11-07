using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Data.DTO.Entities
{
    public class LoggedUserDTO
    {
        public string Token { get; set; } = string.Empty;
        public MemberDTO? LoggedMember { get; set; }
        public LoggedUserDTO(string token, MemberDTO? loggedMember)
        {
            Token = token;
            LoggedMember = loggedMember;
        }
    }
}
