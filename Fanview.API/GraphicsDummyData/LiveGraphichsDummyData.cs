using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;


namespace Fanview.API.GraphicsDummyData
{
    public class LiveGraphichsDummyData
    {
        public LiveStatus GetDummyLiveStatus()
        {
            //var liveStatus = new LiveStatus()
            //{
            //    MatchName = "Day-1",
            //    MatchID = "1",
            //    Teams = new List<LiveTeam>
            //    {
            //        new LiveTeam
            //        {
            //            Name="Team1",
            //            Id="1",
            //            TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                },
            //            }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team2",
            //            Id="2",
            //            TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=true
            //                } ,
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=false
            //               },
            //                }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team3",
            //            Id="3",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team4",
            //            Id="4",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //        new LiveTeam
            //        {
            //            Name="Team5",
            //            Id="5",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //        new LiveTeam
            //        {
            //            Name="Team6",
            //            Id="6",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //        new LiveTeam
            //        {
            //            Name="Team7",
            //            Id="7",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //        new LiveTeam
            //        {
            //            Name="Team8",
            //            Id="8",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //        new LiveTeam
            //        {
            //            Name="Team9",
            //            Id="9",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team10",
            //            Id="10",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team11",
            //            Id="11",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team12",
            //            Id="12",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //         new LiveTeam
            //        {
            //            Name="Team13",
            //            Id="13",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team14",
            //            Id="14",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team15",
            //            Id="15",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team16",
            //            Id="16",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team17",
            //            Id="17",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },
            //          new LiveTeam
            //        {
            //            Name="Team18",
            //            Id="18",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=true
            //                },
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=false
            //                }}
            //        },
            //              new LiveTeam
            //            {
            //                Name="Team19",
            //                Id="19",
            //                   TeamPlayers=new List<LiveTeamPlayers>
            //                {
            //                    new LiveTeamPlayers
            //                    {
            //                        PlayerName="player1",
            //                        PlayeId=1,
            //                        PlayerStatus=true
            //                    },
            //                     new LiveTeamPlayers
            //                    {
            //                        PlayerName="player2",
            //                        PlayeId=2,
            //                        PlayerStatus=true
            //                    },
            //                     new LiveTeamPlayers
            //                    {
            //                        PlayerName="player3",
            //                        PlayeId=3,
            //                        PlayerStatus=true
            //                    },
            //                     new LiveTeamPlayers
            //                    {
            //                        PlayerName="player4",
            //                        PlayeId=4,
            //                        PlayerStatus=false
            //                    } }
            //            },
            //          new LiveTeam
            //        {
            //            Name="Team20",
            //            Id="20",
            //               TeamPlayers=new List<LiveTeamPlayers>
            //            {
            //                new LiveTeamPlayers
            //                {
            //                    PlayerName="player1",
            //                    PlayeId=1,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player3",
            //                    PlayeId=3,
            //                    PlayerStatus=true
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player2",
            //                    PlayeId=2,
            //                    PlayerStatus=false
            //                },
            //                 new LiveTeamPlayers
            //                {
            //                    PlayerName="player4",
            //                    PlayeId=4,
            //                    PlayerStatus=false
            //                } }
            //        },

            //    },


            //};

            return null; //liveStatus;



        }
        public LivePlayerStats GetDummyLiveplayerstats()
        {
            var Liveplayerstats = new LivePlayerStats
            {
                MatchName = "Match1",
                MatchdID = "1",
                PlayerID = 1,
                PlayerName = "Player1",
                Teamid = 1,
                TeamName = "Team1",
                PlayerStats = new PlayerStats
                {
                    Kills = 1,
                    DamageDealt = 2.0
                },
                TeamStats = new LiveTeamStats
                {
                    MatchId = 1,
                    MatchName = "Match1",
                    TeamId = 1,
                    TeamName = "Team1",
                    stats = new PlayerStats
                    {
                        Kills = 1,
                        DamageDealt = 2.0
                    }
                }

            };
            return Liveplayerstats;
        }
     

        public TeamLanding GetTeamLanding()
        {
            var teamlanding = new TeamLanding
            {
                MatchName = "Match1",
                MatchdId = 1,
                Landing = new Landing()
                {
                    TeamName = "Team1",
                    TeamID = "1",
                    Players = new List<LiveTeamPlayers>
                    {
                        new LiveTeamPlayers
                        {
                            PlayeId="1",
                            PlayerName="Player1",
                            location=new LiveLocation
                            {
                                X=1.0,
                                Y=2.0,
                                Z=3.0
                            }
                        },
                         new LiveTeamPlayers
                        {
                            PlayeId="2",
                            PlayerName="Player2",
                            location=new LiveLocation
                            {
                                X=2.0,
                                Y=2.0,
                                Z=4.0
                            }
                        },
                          new LiveTeamPlayers
                        {
                            PlayeId="3",
                            PlayerName="Player3",
                            location=new LiveLocation
                            {
                                X=3.0,
                                Y=4.0,
                                Z=5.0
                            }

                        },
                           new LiveTeamPlayers
                        {
                            PlayeId="4",
                            PlayerName="Player4",
                            location=new LiveLocation
                            {
                                X=4.0,
                                Y=5.0,
                                Z=3.0
                            }
                        }
                    }

                }
            };
            return  teamlanding;
        }
        public TeamRoute GetTeamRoute()
        {
            //var teamrout = new TeamRoute
            //{
            //    MatchName = "Match1",
            //    MatchId = 1,
            //    Routs = new Route
            //    {
            //        TeamID = "1",
            //        TeamName = "Team1",
            //        TeamRank = "3",
            //        TeamRoute = new LiveLocation
            //        {
            //            X = 1.0,
            //            Y = 2.0,
            //            Z = 4.0
            //        },
            //        Safezone = new Safezone
            //        {
            //            SafeZoneId = 1,
            //            Position = new LiveLocation
            //            {
            //                X = 2.0,
            //                Y = 3.0,
            //                Z = 4.0
            //            },
            //            Radius = 4.0
            //        }



            //    }
            //};
            return null; //teamrout;
        }
        public LiveKillList GetLiveKillList()
        {
            var livekilllist = new LiveKillList
            {
                MatchName = "TPP Round 1",
                MatchId = 1,
                DamageLists = new List<DamageList>
                {
                    new DamageList
                    {
                        PlayerRank=1,
                        PlayerName="POLLADERUC",
                        PlayerId=1,
                        Kills=10,
                        TeamId =1
                    },
                    new DamageList
                    {
                        PlayerRank= 2,
                        PlayerName="4AMCPT",
                        PlayerId=1,
                        Kills=9,
                        TeamId= 2


                    },
                    new DamageList
                    {
                        PlayerRank=2,
                        PlayerName="OMGxiaohaixxxx",
                        PlayerId=10,
                        Kills=8,
                        TeamId=3
                    },
                    new DamageList
                    {
                        PlayerRank=4,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        Kills=7,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=5,
                        PlayerName="MiracU",
                        PlayerId=17,
                        Kills=6,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=6,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        Kills=5,
                        TeamId=6
                    },
                    new DamageList
                    {
                        PlayerRank=7,
                        PlayerName="Mossy",
                        PlayerId=25,
                        Kills=4,
                        TeamId=7
                    },
                    new DamageList
                    {
                        PlayerRank=8,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        Kills=3,
                        TeamId=8
                    },
                     new DamageList
                    {
                        PlayerRank=9,
                        PlayerName="MiracU",
                        PlayerId=17,
                        Kills=2,
                        TeamId=9
                    },
                     new DamageList
                    {
                        PlayerRank=10,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        Kills=1,
                        TeamId=10
                    },


                }
            };
            return livekilllist;
        }
        public FlightPath GetFlightPath()
        {
            var flightPath = new FlightPath
            {
                MatchName = "TPP Round 1",
                MatchId = 1,
                FlightPathStart = new LiveLocation
                {
                    X = 416793.3125,
                    Y = 0,
                    Z = 150088
                },
                FlightPathEnd = new LiveLocation
                {
                    X = 351509.375,
                    Y = 439020.15625,
                    Z = 150088
                }

            };
            return flightPath;
        }
        public KillLeaderList GetKillLeaderlist()
        {
            var killerleaderlist = new KillLeaderList
            {
                MatchName = "TPP Round 1",
                MatchID = 1,
                PlayerList = new List<DamageList>
                {
                    new DamageList
                    {
                        PlayerRank=1,
                        PlayerName="POLLADERUC",
                        PlayerId=1,
                        DamageDealt=208.603271,
                        TimeSurvived = 1460,
                        TeamId=1
                    },
                    new DamageList
                    {
                        PlayerRank= 2,
                        PlayerName="4AMCPT",
                        Kills = 0,
                        TimeSurvived = 1300,
                        PlayerId=1,
                        TeamId= 2


                    },
                    new DamageList
                    {
                        PlayerRank=2,
                        PlayerName="OMGxiaohaixxxx",
                        PlayerId=10,
                        Kills = 5,
                        TimeSurvived = 1200,
                        TeamId=3
                    },
                    new DamageList
                    {
                        PlayerRank=4,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        Kills = 5,
                        TimeSurvived = 1200,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=5,
                        PlayerName="MiracU",
                        PlayerId=17,
                        Kills = 10,
                        TimeSurvived = 1704,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=6,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        Kills = 0,
                        TimeSurvived = 1001,
                        TeamId=6
                    },
                    new DamageList
                    {
                        PlayerRank=7,
                        PlayerName="Mossy",
                        PlayerId=25,
                        Kills = 1,
                        TimeSurvived = 995,
                        TeamId=7
                    },
                    new DamageList
                    {
                        PlayerRank=8,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        DamageDealt=193,
                        Kills = 5,
                        TimeSurvived = 1200,
                        TeamId=8
                    },
                     new DamageList
                    {
                        PlayerRank=9,
                        PlayerName="MiracU",
                        PlayerId=17,
                        Kills = 0,
                        TimeSurvived = 1297,
                        TeamId=9
                    },
                     new DamageList
                    {
                        PlayerRank=10,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        Kills = 0,
                        TimeSurvived = 1128,
                        TeamId=10
                    }


                }

            };
            return killerleaderlist;
        }
        public LiveDamageList GetDamagelist()
        {
            var Livedamagelist = new LiveDamageList()
            {
                MatchName = "TPP Round 1",
                MatchId = 1,
                DamageList = new List<DamageList>
                {
                    new DamageList
                    {
                        PlayerRank=1,
                        PlayerName="POLLADERUC",
                        PlayerId=1,
                        DamageDealt=543,
                        TeamId=1
                    },
                    new DamageList
                    {
                        PlayerRank= 2,
                        PlayerName="4AMCPT",
                        PlayerId=1,
                        DamageDealt=493,
                        TeamId= 2


                    },
                    new DamageList
                    {
                        PlayerRank=2,
                        PlayerName="OMGxiaohaixxxx",
                        PlayerId=10,
                        DamageDealt=443,
                        TeamId=3
                    },
                    new DamageList
                    {
                        PlayerRank=4,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        DamageDealt=393,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=5,
                        PlayerName="MiracU",
                        PlayerId=17,
                        DamageDealt=343,
                        TeamId=4
                    },
                    new DamageList
                    {
                        PlayerRank=6,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        DamageDealt=293,
                        TeamId=6
                    },
                    new DamageList
                    {
                        PlayerRank=7,
                        PlayerName="Mossy",
                        PlayerId=25,
                        DamageDealt=243,
                        TeamId=7
                    },
                    new DamageList
                    {
                        PlayerRank=8,
                        PlayerName="Jeemzz",
                        PlayerId=14,
                        DamageDealt=193,
                        TeamId=8
                    },
                     new DamageList
                    {
                        PlayerRank=9,
                        PlayerName="MiracU",
                        PlayerId=17,
                        DamageDealt=143,
                        TeamId=9
                    },
                     new DamageList
                    {
                        PlayerRank=10,
                        PlayerName="Voxsic",
                        PlayerId=21,
                        DamageDealt=93,
                        TeamId=10
                    },


                }
            };
            return Livedamagelist;
        }
        public IEnumerable<KillZone> GetLiveKillzone()
        {
            //var killZones = new List<KillZone>
            //{
            //    new KillZone{
            //    MatchId= "5b45e498040bbeb6a89877f6",
            //    MatchName="Day-1",
            //    Location=new LiveLocation
            //    {
            //        X =574472,
            //        Y=306982.281,
            //        Z=1208.98
            //    },
            //    KillerName="DTN_iLGO",
            //    KillerId=1,
            //    TeamId=4

            //    },
            //    new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-1-1",
            //    Location=new LiveLocation
            //    {
            //        X =574533.3,
            //        Y=302502.062,
            //        Z=1078.32
            //    },
            //    KillerName="Lunatichai_Tio",
            //    KillerId=3,
            //    TeamId=5

            //    },
            //    new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-1-2",
            //    Location=new LiveLocation
            //    {
            //        X =559835.438,
            //        Y=306535.3,
            //        Z=1602.49
            //    },
            //    KillerName="Erangel_Main",
            //    KillerId=4,
            //    TeamId=2

            //    },
            // new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-2-3",
            //    Location=new LiveLocation
            //    {
            //        X =527530.3,
            //        Y= 322461.9,
            //        Z=928.57
            //    },
            //    KillerName="KDG_Phenom",
            //    KillerId=5,
            //    TeamId=3


            //    },
            //       new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-2-3",
            //    Location=new LiveLocation
            //    {
            //        X =562411.5,
            //        Y= 306754.438,
            //        Z=1252.07
            //    },
            //    KillerName="ACTOZ_I_Jaehyeon",
            //    KillerId=6,
            //    TeamId=10


            //    },
            //            new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-2-5",
            //    Location=new LiveLocation
            //    {
            //        X =562411.5,
            //        Y= 306754.438,
            //        Z=1252.07
            //    },
            //    KillerName= "LH-FLUX_Retri",
            //    KillerId=7,
            //    TeamId=15


            //    },
            //    new KillZone{
            //    MatchId= "f84d39a1-8218-4438-9bf5-7150f9e0f093",
            //    MatchName="Day-2-3",
            //    Location=new LiveLocation
            //    {
            //        X =527530.3,
            //        Y= 322461.9,
            //        Z=928.57
            //    },
            //    KillerName= "LH-FLUX_Retri",
            //    KillerId =8,
            //    TeamId=5


            //    },


            // };
            return null; //killZones;
        }
    }
}
