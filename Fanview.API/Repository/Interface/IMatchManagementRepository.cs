﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchManagementRepository
    {
        Task<IEnumerable<Event>> GetMatchDetails();

        void PostMatchDetails(Event model);

        Task<Object> GetTournaments();

        Task<Object> GetTournamentsMatches(string tournamentName);
    }
}
