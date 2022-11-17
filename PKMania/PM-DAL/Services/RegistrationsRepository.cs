
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";
        public IEnumerable<Player> GetAllRegistrationsForOneTournament(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT r.player_id,m.pseudo,r.table_nr,r.stack,r.bonus_time,r.eliminated_at FROM [Registrations] r 
                                JOIN [Members] m ON r.player_id = m.id
                                WHERE r.tournament_id = @trId";
            cmd.Parameters.AddWithValue("trId", trId);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime elim = new DateTime(1970, 1, 1, 0, 0, 0);
                if (reader["eliminated_at"] != DBNull.Value) { elim = (DateTime)reader["eliminated_at"]; }
                yield return new Player
                {
                    Id = (int)reader["player_id"],
                    Pseudo = (string)reader["pseudo"],
                    TableNr = (int)reader["table_nr"],
                    Stack = (int)reader["stack"],
                    BonusTime = (int)reader["bonus_time"],
                    EliminatedAt = elim
                };
            }
        }

        public Boolean IsPlayerRegistered(int trId, int playerId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM [Registrations] WHERE tournament_id = @trId AND player_id = @playerId";
            cmd.Parameters.AddWithValue("trId",trId);
            cmd.Parameters.AddWithValue("playerId", playerId);
            int cpt = (int)cmd.ExecuteScalar();
            return cpt > 0;
        }
        public void UnregisterTournament(int trId, int playerId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Registrations] WHERE tournament_id = @trId AND player_id = @playerId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.Parameters.AddWithValue("playerId", playerId);
            cmd.ExecuteNonQuery();
        }
    }
}
