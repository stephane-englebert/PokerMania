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
        void UpdateCurrentTournIdForOneTournament(int trId);
        int GetMemberCurrentTournId(int playerId);
        void SetMemberCurrentTournId(int trId, int playerId);
        void SetAllRegisteredMembersCurrentTournId(int trId);
        void SetPlayerIsConnected(int playerId);
        void SetPlayerIsDisconnected(int playerId);
        IEnumerable<int> GetIdOfDisconnectedPlayers(int trId);
    }
}
