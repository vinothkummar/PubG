using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.TestData
{
    public class LiveGraphichsDummyData
    {
        public LiveStatus GetDummyLiveStatus()
        {
            var livestatue = new LiveStatus()
            {
                MatchName = "Day-1",
                MatchID = 1,
                Teams = new List<LiveTeam>
                {
                    new LiveTeam
                    {
                        Name="LiveTeam1",
                        Id="1",
                        TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            },
                        }
                    },
                      new LiveTeam
                    {
                        Name="Team2",
                        Id="2",
                        TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team3",
                        Id="3",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team4",
                        Id="4",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                    new LiveTeam
                    {
                        Name="Team5",
                        Id="5",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                    new LiveTeam
                    {
                        Name="Team6",
                        Id="6",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                    new LiveTeam
                    {
                        Name="Team7",
                        Id="7",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                    new LiveTeam
                    {
                        Name="Team8",
                        Id="8",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                    new LiveTeam
                    {
                        Name="Team9",
                        Id="9",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team10",
                        Id="10",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team11",
                        Id="11",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team12",
                        Id="12",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                     new LiveTeam
                    {
                        Name="Team13",
                        Id="13",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team14",
                        Id="14",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team15",
                        Id="15",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team16",
                        Id="16",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team17",
                        Id="17",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team18",
                        Id="18",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team19",
                        Id="19",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            } }
                    },
                      new LiveTeam
                    {
                        Name="Team20",
                        Id="20",
                           TeamPlayers=new List<LiveTeamPlayers>
                        {
                            new LiveTeamPlayers
                            {
                                PlayerName="player1",
                                PlayeId=1,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player2",
                                PlayeId=2,
                                PlayerStatus=false
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player3",
                                PlayeId=3,
                                PlayerStatus=true
                            },
                             new LiveTeamPlayers
                            {
                                PlayerName="player4",
                                PlayeId=4,
                                PlayerStatus=false
                            }
                         }
                    }

                }
            };
            var LiveStatue = livestatue.Teams.Select(x => x.TeamPlayers.OrderBy(m => m.PlayerStatus));
            return livestatue;
        }

        //public LivePlayerStats GetDummyLiveplayerstats()
        //{
        //    var Liveplayerstats = new LivePlayerStats
        //    {
        //        MatchName = "Match1",
        //        MatchdID = "1",
        //        PlayerID = 1,
        //        PlayerName = "Player1",
        //        Teamid = 1,
        //        TeamName = "Team1",
        //        PlayerStats = new PlayerStats
        //        {
        //            Kills = 1,
        //            DamageDealt = 2.0
        //        },
        //        TeamStats = new LiveTeamStats
        //        {
        //            MatchId = 1,
        //            MatchName = "Match1",
        //            TeamId = 1,
        //            TeamName = "Team1",
        //            stats = new PlayerStats
        //            {
        //                Kills = 1,
        //                DamageDealt = 2.0
        //            }
        //        }

        //    };
        //    return Liveplayerstats;
        //}
        //public KillZone GetLiveKillzone()
        //{
        //    var LiveKillZone = new KillZone
        //    {
        //        MatchName = "Match1",
        //        MatchId = 1,
        //        kills = new int[] { 76, 77, 78, 79, 80 },
        //        KillerName = "Player1",
        //        Location = new location
        //        {
        //            X = 1.0,
        //            Y = 1.0,
        //            Z = 1.0
        //        }

        //    };
        //    return LiveKillZone;
        //}
        //public TeamLanding GetTeamLanding()
        //{
        //    var teamlanding = new TeamLanding
        //    {
        //        MatchName = "Match1",
        //        MatchdId = 1,
        //        Landing = new Landing()
        //        {
        //            TeamName = "Team1",
        //            TeamID = "1",
        //            Players = new List<TeamPlayer>
        //            {
        //                new TeamPlayer
        //                {
        //                    PlayeId=1,
        //                    PlayerName="Player1",
        //                    location=new location
        //                    {
        //                        X=1.0,
        //                        Y=2.0,
        //                        Z=3.0
        //                    }
        //                },
        //                 new TeamPlayer
        //                {
        //                    PlayeId=2,
        //                    PlayerName="Player2",
        //                    location=new location
        //                    {
        //                        X=2.0,
        //                        Y=2.0,
        //                        Z=4.0
        //                    }
        //                },
        //                  new TeamPlayer
        //                {
        //                    PlayeId=3,
        //                    PlayerName="Player3",
        //                    location=new location
        //                    {
        //                        X=3.0,
        //                        Y=4.0,
        //                        Z=5.0
        //                    }

        //                },
        //                   new TeamPlayer
        //                {
        //                    PlayeId=4,
        //                    PlayerName="Player4",
        //                    location=new location
        //                    {
        //                        X=4.0,
        //                        Y=5.0,
        //                        Z=3.0
        //                    }
        //                }
        //            }

        //        }
        //    };
        //    return teamlanding;
        //}
        //public TeamRoute GetTeamRoute()
        //{
        //    var teamrout = new TeamRoute
        //    {
        //        MatchName = "Match1",
        //        MatchId = 1,
        //        Routs = new Route
        //        {
        //            TeamID = "1",
        //            TeamName = "Team1",
        //            TeamRank = "3",
        //            TeamRoute = new location
        //            {
        //                X = 1.0,
        //                Y = 2.0,
        //                Z = 4.0
        //            },
        //            Safezone = new Safezone
        //            {
        //                SafeZoneId = 1,
        //                Position = new location
        //                {
        //                    X = 2.0,
        //                    Y = 3.0,
        //                    Z = 4.0
        //                },
        //                Radius = 4.0
        //            }



        //        }
        //    };
        //    return teamrout;
        //}
        //public LiveKillList GetLiveKillList()
        //{
        //    var livekilllist = new LiveKillList
        //    {
        //        MatchName = "Match1",
        //        MatchId = 1,
        //        DamageLists = new List<DamageList>
        //        {
        //            new DamageList
        //            {
        //                PlayerRank=1,
        //                PlayerName="Player1",
        //                PlayerId=1,
        //                DamageDealt=10.0
        //            },
        //            new DamageList
        //            {
        //                PlayerRank=4,
        //                PlayerName="Player2",
        //                PlayerId=2,
        //                DamageDealt=3.0
        //            },
        //            new DamageList
        //            {
        //                PlayerRank=2,
        //                PlayerName="Player3",
        //                PlayerId=3,
        //                DamageDealt=8.0
        //            },
        //            new DamageList
        //            {
        //                PlayerRank=3,
        //                PlayerName="Player4",
        //                PlayerId=4,
        //                DamageDealt=5.0
        //            },

        //        }
        //    };
        //    return livekilllist;
        //}
        //public FlightPath GetFlightPath()
        //{
        //    var flightPath = new FlightPath
        //    {
        //        MatchName = "Match1",
        //        MatchId = 1,
        //        FlightPathStart = new location
        //        {
        //            X = 2.0,
        //            Y = 3.0,
        //            Z = 4.0
        //        },
        //        FlightPathEnd = new location
        //        {
        //            X = 4.0,
        //            Y = 5.0,
        //            Z = 6.0
        //        }

        //    };
        //    return flightPath;
        //}
        //public KillLeaderList GetKillLeaderlist()
        //{
        //    var killerleaderlist = new KillLeaderList
        //    {
        //        MatchName = "Match1",
        //        MatchID = 1,
        //        KillLeader = new KillLeader
        //        {
        //            kills = new int[] { 76, 77, 78, 79 },
        //            DamageDealt = 3.0,
        //            SurvivlTime = new TimeSpan(9, 0, 0),
        //        }
        //    };
        //    return killerleaderlist;
        //}


    }
}

