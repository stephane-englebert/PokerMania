
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class HandsRepository : IHandsRepository
    {
        private string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";
        public HandsRepository()
        {

        }
        public void DeleteHandsByTournament(int trId){
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Hands] WHERE tournament_id = @trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.ExecuteNonQuery();
        }
    }
}
