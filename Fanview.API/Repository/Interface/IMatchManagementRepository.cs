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
    }
}