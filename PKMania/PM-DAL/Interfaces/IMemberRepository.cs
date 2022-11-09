using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface IMemberRepository
    {
        Member GetMemberByCredentials(string userIdentifier, string password);
        Member GetMemberByIdentifier(string userIdentifier);
        void AddMember(Member member, byte[] password, byte[] salt);
        bool ExistEmail(string email);
        bool ExistPseudo(string pseudo);
    }
}
