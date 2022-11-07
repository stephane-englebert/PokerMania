
using PM_DAL.Data.Entities;

namespace PM_BLL.Interfaces
{
    public interface ISecurityTokenService
    {
        string GetNewSecurityToken(Member member);
    }
}
