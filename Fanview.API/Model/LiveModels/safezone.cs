﻿namespace Fanview.API.Model.LiveModels
{
    public class Safezone
    {
        public int SafeZoneId { get; set; }
        public double Radius { get; set; }
        public LiveLocation Position { get; set; }

    }
}