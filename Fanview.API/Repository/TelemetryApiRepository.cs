﻿using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Fanview.API.Repository
{
    public class TelemetryApiRepository : ITelemetryApiRepository
    {
        private IGenericRepository<PlayerKill> _genericRepository;

        public TelemetryApiRepository(IGenericRepository<PlayerKill> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public Task<IEnumerable<PlayerKill>> GetPlayerKills()
        {
            
            var result = _genericRepository.GetAll("killMessages");

            return result;
        }
    }
}