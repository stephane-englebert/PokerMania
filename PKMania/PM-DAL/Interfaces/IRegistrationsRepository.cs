﻿
using PM_DAL.Data.Entities;

namespace PM_DAL.Interfaces
{
    public interface IRegistrationsRepository
    {
        IEnumerable<Player> GetAllRegistrationsForOneTournament(int trId);
    }
}
