﻿
using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface IRegistrationsRepository
    {
        IEnumerable<Player> GetAllRegistrationsForOneTournament(int trId);
        Boolean IsPlayerRegistered(int trId, int playerId);
    }
}
