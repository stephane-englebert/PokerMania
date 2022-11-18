﻿using PM_DAL.Interfaces;
using System.Data.SqlClient;

namespace PM_DAL.Services
{
    public class TournamentRepository: ITournamentRepository
    {
        private readonly string connectionString = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = Pokermania; Integrated Security = True;";

        public void CreateTournament(DateTime startDate, string name, int type, int prizePool, int gainsSharingNr)        
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Tournaments] (status,started_on,name,tournament_type_id,players_nb,prize_pool,gains_sharing_nr)
                                VALUES('created',@startDate,@name,@type,0,@prizePool,@gainsSharingNr)";
            cmd.Parameters.AddWithValue("startDate", startDate);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("type", type);
            cmd.Parameters.AddWithValue("prizePool", prizePool);
            cmd.Parameters.AddWithValue("gainsSharingNr", gainsSharingNr);
            cmd.ExecuteNonQuery();
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
    }
}
