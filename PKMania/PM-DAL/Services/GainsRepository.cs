
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class GainsRepository : IGainsRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";
        public IEnumerable<Gains> GetAllGainsSharings()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT id,gains_sharing_nr,start_place,end_place,percentage FROM [Gains];";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Gains(
                    (Int16)reader["gains_sharing_nr"],
                    (Int16)reader["start_place"],
                    (Int16)reader["end_place"],
                    0,
                    (decimal)reader["percentage"]
                );
            }
        }
    }
}
