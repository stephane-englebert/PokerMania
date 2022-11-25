
namespace PM_BLL.Data.DTO.Entities.Hubs
{
    public class RoomConnection
    {
        public string RoomName { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public int TournamentId { get; set; }
        public int PlayerId { get; set; }
        public RoomConnection(string roomName, string connectionId, int tournamentId, int playerId)
        {
            RoomName = roomName;
            ConnectionId = connectionId;
            TournamentId = tournamentId;
            PlayerId = playerId;
        }
    }
}
