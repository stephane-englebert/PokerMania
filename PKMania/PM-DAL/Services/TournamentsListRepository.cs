using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class TournamentsListRepository : ITournamentsListRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;"; 
        public IEnumerable<Tournament> GetActiveTournaments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT id,status,started_on,finished_on,name,tournament_type_id FROM [Tournaments]  WHERE status IN ('created','waitingForPlayers','ongoing','paused');";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Tournament
                {
                    Id = (int)reader["id"],
                    Status = (string)reader["status"],
                    StartedOn = (DateTime)reader["started_on"],
                    FinishedOn = Convert.ToDateTime("1970-01-01 00:00:00.0000000"),
                    Name = (string)reader["name"],
                    TournamentType = (int)reader["tournament_type_id"],
                    RegistrationsNumber = 0,
                    RealPaidPlaces = 0,
                    Gains = new List<Gains>(),
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
    }
}
