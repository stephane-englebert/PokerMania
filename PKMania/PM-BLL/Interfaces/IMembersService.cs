using PM_BLL.Data.DTO.Forms;

namespace PM_BLL.Interfaces
{
    public interface IMembersService
    {
        void AddMember(MemberRegisterFormDTO member);
        void SetAllRegisteredMembersCurrentTournId(int trId);
        void SetPlayerIsConnected(int playerId);
        void SetPlayerIsDisconnected(int playerId);
        IEnumerable<int> GetIdOfDisconnectedPlayers(int trId);
    }
}
