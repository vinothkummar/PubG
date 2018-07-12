namespace Fanview.API.Model.LiveModels { 
using System;


public class KillLeader
{
    public int[] kills { get; set; }
    public double DamageDealt { get; set; }
    public TimeSpan SurvivlTime { get; set; }
}
}