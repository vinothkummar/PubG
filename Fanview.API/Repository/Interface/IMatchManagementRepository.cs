﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using MongoDB.Driver;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchManagementRepository
    {
        Task<IEnumerable<Event>> GetMatchDetails();

        void PostMatchDetails(Event model);

        Task<Object> GetTournaments();

        Task<Object> GetTournamentsMatches(string tournamentName);

        Task<Object> GetLiveLatestMatch(string tournamentName);

        dynamic DeleteDocumentCollections(string matchId);

        dynamic DeleteLiveDataDocument();

        Task<IEnumerable<DeskSeatings>> GetPlayerDeskPositions();

        void CreatePlayerDeskPosition(IEnumerable<DeskSeatings> seatingPosition);

        void EditPlayerDeskPosition(IEnumerable<DeskSeatings> seatingPosition);
    }
}
