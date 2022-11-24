using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class TournamentRepository: ITournamentRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";

        public int CreateTournament(DateTime startDate, string name, int type, int prizePool, int gainsSharingNr)        
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Tournaments] (status,started_on,name,tournament_type_id,players_nb,prize_pool,gains_sharing_nr)
            output inserted.id
            VALUES('created',@startDate,@name,@type,0,@prizePool,@gainsSharingNr)";
            cmd.Parameters.AddWithValue("startDate", startDate);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("type", type);
            cmd.Parameters.AddWithValue("prizePool", prizePool);
            cmd.Parameters.AddWithValue("gainsSharingNr", gainsSharingNr);
            return (int)cmd.ExecuteScalar();
        }
        public void DeleteTournament(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Tournaments] WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.ExecuteNonQuery();
        }
        public void UpdateNumberPlayersRegistered(int trId,int incOrDec)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();                
            cmd.CommandText = @"UPDATE [Tournaments] SET players_nb = players_nb + " + incOrDec + " WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.ExecuteNonQuery();
        }

        public Boolean CanJoinLobby(int trId, int playerId)
        {            
            string trStatus = this.GetTournamentStatus(trId);
            return  trStatus == "ongoing" || trStatus == "waitingForPlayers";
        }

        public string GetTournamentStatus(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT status FROM [Tournaments] WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            return (string)cmd.ExecuteScalar();
        }
        public void SetTournamentStatus(int trId, string status)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE [Tournaments] SET status = @status WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.Parameters.AddWithValue("status", status);
            cmd.ExecuteNonQuery();
        }
        public void LaunchTournament(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE [Tournaments] SET started_on = @startedOn,status = 'waitingForPlayers' WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.Parameters.AddWithValue("startedOn", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
        public void StartTournament(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE [Tournaments] SET started_on = @startedOn,status = 'ongoing' WHERE id=@trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.Parameters.AddWithValue("startedOn", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
        public IEnumerable<Clean> GetIdTournamentsToClean()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT id,status FROM [Tournaments] WHERE status IN ('canceled','finished')";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Clean(
                    reader.GetInt32(0),
                    reader.GetString(1)
                );
            }
        }
    }
}
