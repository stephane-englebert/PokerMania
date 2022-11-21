using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace PM_DAL.Services
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";

        public MemberRepository()
        {
            //_configuration = configuration;
            //this.connectionString = _configuration.GetConnectionString("PkMania");
        }

        public Member GetMemberByCredentials(string ident, string pwd)
        {
            Member mb = GetMemberByIdentifier(ident);
            if(mb == null) { return null; }
            byte[] salt = (byte[])mb.Salt;
            byte[] hashedPwd = KeyDerivation.Pbkdf2(
                password: pwd,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE password = @hashedPwd AND (pseudo = @ident OR email = @ident)";
            cmd.Parameters.AddWithValue("hashedPwd", hashedPwd);
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
                    Salt = (byte[])reader["salt"],
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
                    Salt = (byte[])reader["salt"],
                    Bankroll = (int)reader["bankroll"],
                    IsPlaying = (bool)reader["playing"],
                    IsDisconnected = (bool)reader["disconnected"],
                    CurrentTournament = (int)reader["current_tournament_id"]
                };
            }
            return null;
        }

        public void AddMember(Member member, byte[] password, byte[] salt)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[Members](role, pseudo, email, password, salt, bankroll, playing, current_tournament_id, disconnected) VALUES(@role,@pseudo,@email,@password, @salt, @bankroll, @playing, @current_tournament_id, @disconnected)";
            cmd.Parameters.AddWithValue("role", member.Role);
            cmd.Parameters.AddWithValue("pseudo", member.Pseudo);
            cmd.Parameters.AddWithValue("email", member.Email);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Parameters.AddWithValue("salt", salt);
            cmd.Parameters.AddWithValue("bankroll", member.Bankroll);
            cmd.Parameters.AddWithValue("playing", member.IsPlaying);
            cmd.Parameters.AddWithValue("current_tournament_id", member.CurrentTournament);
            cmd.Parameters.AddWithValue("disconnected", member.IsDisconnected);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public bool ExistEmail(string email)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE email=@email";
            cmd.Parameters.AddWithValue("email", email);
            bool emailFound = false;
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { emailFound = true; }
            return emailFound;
        }

        public bool ExistPseudo(string pseudo)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE pseudo=@pseudo";
            cmd.Parameters.AddWithValue("pseudo", pseudo);
            bool pseudoFound = false;
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { pseudoFound = true; }
            return pseudoFound;
        }
        public void UpdateCurrentTournIdForOneTournament(int trId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE [Members] SET current_tournament_id = 0 WHERE current_tournament_id = @trId";
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.ExecuteNonQuery();
        }

        public int GetMemberCurrentTournId(int playerId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT current_tournament_id FROM [Members] WHERE id = @playerId";
            cmd.Parameters.AddWithValue("playerId", playerId);
            return (int)cmd.ExecuteScalar();
        }

        public void SetMemberCurrentTournId(int trId, int playerId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE [Members] SET current_tournament_id = @trId WHERE id = @playerId";
            cmd.Parameters.AddWithValue("playerId", playerId);
            cmd.Parameters.AddWithValue("trId", trId);
            cmd.ExecuteNonQuery();
        }
        public void GetMemberIdOfPlayersJoiningTournament(int trId)
        {

        }
    }
}
