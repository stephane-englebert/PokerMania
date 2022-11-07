using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.DTO.Forms;
using PM_BLL.Interfaces;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Security.Authentication;

namespace PM_BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ISecurityTokenService _securityTokenService;

        public AuthService(IMemberRepository memberRepository, ISecurityTokenService securityTokenService)
        {
            _memberRepository = memberRepository;
            _securityTokenService = securityTokenService;
        }

        public LoggedUserDTO UserLogin(MemberLoginFormDTO member)
        {
            Member memb = _memberRepository.GetMemberByCredentials(member.UserIdentifier,member.Password);
            if (memb == null)
            {
                memb = _memberRepository.GetMemberByIdentifier(member.UserIdentifier);
                if(memb == null)
                {
                    throw new AuthenticationException("AUTH_IDENT_KO");
                }
                else
                {
                    throw new AuthenticationException("AUTH_PWD_KO");
                }
            }
            MemberDTO membDTO = new MemberDTO(memb);
            string token = _securityTokenService.GetNewSecurityToken(memb);
            LoggedUserDTO loggedUser = new LoggedUserDTO(token, membDTO);
            return loggedUser;
        }
    }
}
