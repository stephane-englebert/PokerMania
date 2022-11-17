using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class TournamentsListRepository : ITournamentsListRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;"; 
        public IEnumerable<Tournament> GetActiveTournaments(IEnumerable<Gains> allGains)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT t.id,t.status,t.started_on,t.finished_on,t.name,t.tournament_type_id,t.players_nb,t.prize_pool,t.gains_sharing_nr                                        
                                FROM [Tournaments] t
                                WHERE t.status IN ('created','waitingForPlayers','ongoing','paused')";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                List<Gains> newGains = new List<Gains>();
                yield return new Tournament
                {
                    Id = (int)reader["id"],
                    Status = (string)reader["status"],
                    StartedOn = (DateTime)reader["started_on"],
                    FinishedOn = Convert.ToDateTime("1970-01-01 00:00:00.0000000"),
                    Name = (string)reader["name"],
                    TournamentType = (int)reader["tournament_type_id"],
                    RegistrationsNumber = (int)reader["players_nb"],
                    PrizePool = (int)reader["prize_pool"],
                    RealPaidPlaces = 0,
                    GainsSharingNr = (Int16)reader["gains_sharing_nr"],
                    Gains = newGains,
                    CurrentLevel = 0,
                    CurrentSmallBlind = 0,
                    CurrentBigBlind = 0,
                    CurrentAnte = 0,
                    NextLevel = 0,
                    TimeBeforeNextLevel = 0,
                    NextSmallBlind = 0,
                    NextBigBlind = 0,
                    NextAnte = 0
                };
            }
        }
        public Boolean IsTournamentStarted(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT status FROM [Tournaments] WHERE id = @trId";
            cmd.Parameters.AddWithValue("trId", trId);
            return (string)cmd.ExecuteScalar() != "created" && (string)cmd.ExecuteScalar() == "waitingForPlayers";
        }
    }
}
