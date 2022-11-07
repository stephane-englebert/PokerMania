using Microsoft.Extensions.Configuration;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";

        public MemberRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            //this.connectionString = _configuration.GetConnectionString("PkMania");
        }

        public Member GetMemberByCredentials(string ident, string pwd)
        {            
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE password = @pwd AND (pseudo = @ident OR email = @ident)";
            cmd.Parameters.AddWithValue("pwd", pwd);
            cmd.Parameters.AddWithValue("ident", ident);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new Member
                {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Bankroll = (int)reader["bankroll"],
                    IsPlaying = (bool)reader["playing"],
                    IsDisconnected = (bool)reader["disconnected"],
                    CurrentTournament = (int)reader["current_tournament_id"]
                };
            }
            return null;
        }

        public Member GetMemberByIdentifier(string ident)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE pseudo = @ident OR email = @ident";
            cmd.Parameters.AddWithValue("ident", ident);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new Member
                {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Bankroll = (int)reader["bankroll"],
                    IsPlaying = (bool)reader["playing"],
                    IsDisconnected = (bool)reader["disconnected"],
                    CurrentTournament = (int)reader["current_tournament_id"]
                };
            }
            return null;
        }

        public void AddMember(Member member, string password)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[Members](role, pseudo, email, password, bankroll, playing, current_tournament_id, disconnected) VALUES(@role,@pseudo,@email,@password,@bankroll, @playing, @current_tournament_id, @disconnected)";
            cmd.Parameters.AddWithValue("role", member.Role);
            cmd.Parameters.AddWithValue("pseudo", member.Pseudo);
            cmd.Parameters.AddWithValue("email", member.Email);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("bankroll", member.Bankroll);
            cmd.Parameters.AddWithValue("playing", member.IsPlaying);
            cmd.Parameters.AddWithValue("current_tournament_id", member.CurrentTournament);
            cmd.Parameters.AddWithValue("disconnected", member.IsDisconnected);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
