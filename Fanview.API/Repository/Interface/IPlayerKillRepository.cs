﻿using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerKillRepository
    {
        void InsertPlayerKillTelemetry(string jsonResult);

        Task<IEnumerable<Kill>> GetPlayerKills();
    }
}
