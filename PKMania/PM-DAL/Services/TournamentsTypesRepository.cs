using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class TournamentsTypesRepository : ITournamentsTypesRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";

        public IEnumerable<TournamentsTypes> GetAllTournamentsTypes()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tournaments_types";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new TournamentsTypes
                {
                    Id = (int)reader["id"],
                    BuyIn = (int)reader["buy_in"],
                    LateRegistrationLevel = (int)reader["late_registration_level"],
                    StartingStack = (int)reader["starting_stack"],
                    Rebuy = (Boolean)reader["rebuy"],
                    RebuyLevel = (int)reader["rebuy_level"],
                    LevelsDuration = (int)reader["levels_duration"],
                    MinPlayers = (int)reader["min_players"],
                    MaxPlayers = (int)reader["max_players"],
                    PlayersPerTable = (int)reader["players_per_table"],
                    MaxPaidPlaces = (int)reader["max_paid_places"],
                    GainsSharingNr = (int)reader["gains_sharing_nr"]
                };
            }
        }
        public TournamentsTypes GetTournamentsTypesById(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Tournaments_types WHERE id = @id";
            cmd.Parameters.AddWithValue("id", id);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TournamentsTypes
                {
                    Id = (int)reader["id"],
                    BuyIn = (int)reader["buy_in"],
                    LateRegistrationLevel = (int)reader["late_registration_level"],
                    StartingStack = (int)reader["starting_stack"],
                    Rebuy = (Boolean)reader["rebuy"],
                    RebuyLevel = (int)reader["rebuy_level"],
                    LevelsDuration = (int)reader["levels_duration"],
                    MinPlayers = (int)reader["min_players"],
                    MaxPlayers = (int)reader["max_players"],
                    PlayersPerTable = (int)reader["players_per_table"],
                    MaxPaidPlaces = (int)reader["max_paid_places"],
                    GainsSharingNr = (int)reader["gains_sharing_nr"]
                };
            }
            return null;
        }
    }
}
