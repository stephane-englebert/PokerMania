using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.DTO.Forms;

namespace PM_BLL.Interfaces
{
    public interface IAuthService
    {
        LoggedUserDTO UserLogin(MemberLoginFormDTO member);
    }
}
