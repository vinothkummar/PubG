﻿using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerVehicleLeaveRepository
    {
        void InsertVehicleLeaveTelemetry(string jsonResult);

        Task<IEnumerable<VehicleLeave>> GetPlayerLeftVechile();
    }
}