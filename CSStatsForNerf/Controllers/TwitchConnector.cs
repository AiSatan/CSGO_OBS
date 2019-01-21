using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSStatsForNerf.Controllers
{
    public static class TwitchConnector
    {
        static object _locker = new object();
        static string gameResult = "-";

        internal static void Execute(dynamic data)
        {
            lock (_locker)
            {
                if ("AiSatan" == data.player?.name?.ToString() && data.map?.phase?.ToString() != "gameover")
                {
                    gameResult = "-";

                    // Kills
                    if (data.map?.phase?.ToString() == "warmup")
                    {
                        AddEvent(EventT.WarmUp);
                    }
                    else
                    {
                        KillEvents(data);
                    }

                    // Bomb stats
                    var bomb = data.round?.bomb?.ToString();
                    if (bomb == "planted")
                    {
                        AddEvent(data.player?.team?.ToString() == "CT" ? EventT.CTPlanted : EventT.TPlanted);
                    }
                    else if (bomb == "exploded")
                    {
                        AddEvent(data.player?.team?.ToString() == "CT" ? EventT.CTExploded : EventT.TExploded);
                    }
                    else if (bomb == "defused")
                    {
                        AddEvent(data.player?.team?.ToString() == "CT" ? EventT.CTDefused : EventT.TDefused);
                    }

                    // One hp
                    if (Convert.ToInt32(data.player?.state?.health) == 1)
                    {
                        AddEvent(EventT.OneHp);
                    }

                    //  Most kills
                    var kills = Convert.ToInt32(data.player?.match_stats?.kills);
                    if (kills >= 10 && kills < 20)
                    {
                        AddEvent(EventT.TenKills);
                    }
                    else if (kills >= 20 && kills < 30)
                    {
                        AddEvent(EventT.TwentyKills);
                    }
                    else if (kills >= 30 && kills < 40)
                    {
                        AddEvent(EventT.ThirtyKills);
                    }
                    else if (kills >= 40)
                    {
                        AddEvent(EventT.FortyKills);
                    }

                    // K/D
                    var death = Convert.ToInt32(data.player?.match_stats?.deaths);
                    if(death >= 2 && kills <= 0)
                    {
                        AddEvent(EventT.UselessScore);
                    }
                    if (death >= 5 && (float)kills/ (float)death < 0.3f)
                    {
                        AddEvent(EventT.UselessKD);
                    }
                    else if((float)kills / (float)death > 3.0f)
                    {
                        AddEvent(EventT.GodKD);
                    }

                }
                else
                {
                    if (Convert.ToInt32(data.map?.team_ct?.score) == 16)
                    {
                        var res = data.player?.team?.ToString() == "CT" ? EventT.Win : EventT.Lose;
                        gameResult = res.ToString();
                        AddEvent(res);
                    }
                    if (Convert.ToInt32(data.map?.team_t?.score) == 16)
                    {
                        var res = data.player?.team?.ToString() == "T" ? EventT.Win : EventT.Lose;
                        gameResult = res.ToString();
                        AddEvent(res);
                    }
                    if (Convert.ToInt32(data.map?.team_t?.score) == 15 && Convert.ToInt32(data.map?.team_ct?.score) == 15)
                    {
                        AddEvent(EventT.Draft);
                    }
                }

                if(data.map?.phase?.ToString() == "gameover")
                {
                    AddEvent(EventT.Gameover);
                }
                else if (data.round?.phase?.ToString() == "freezetime")
                {
                    AddEvent(EventT.RoundStart);
                }



                var path = @"C:\Users\aisat\Documents\Visual Studio 2017\Projects\CSGOToTwitch\CSStatsForNerf\bin\Debug\netcoreapp2.1\stats.txt";

                File.WriteAllText(path, "");
                File.AppendAllText(path, "map.phase " + data.map?.phase ?? "null");
                File.AppendAllText(path, "\r\nGame Result: " + gameResult);
                File.AppendAllText(path, "\r\nmap.round " + data.map?.round ?? "null");
                File.AppendAllText(path, "\r\nmap.team_ct.score " + data.map?.team_ct?.score ?? "null");
                File.AppendAllText(path, "\r\nmap.team_t.score " + data.map?.team_t?.score ?? "null");
                File.AppendAllText(path, "\r\nround.win_team " + data.round?.win_team ?? "null");
                File.AppendAllText(path, "\r\nround.bomb " + data.round?.bomb ?? "null");
                File.AppendAllText(path, "\r\nplayer.activity " + data.player?.activity ?? "null");
                File.AppendAllText(path, "\r\nplayer.health " + data.player?.state?.health ?? "null");
                File.AppendAllText(path, "\r\nplayer.armor " + data.player?.state?.armor ?? "null");
                File.AppendAllText(path, "\r\nplayer.helmet " + data.player?.state?.helmet ?? "null");
                File.AppendAllText(path, "\r\nplayer.money " + data.player?.state?.money ?? "null");
                File.AppendAllText(path, "\r\nplayer.round_kills " + data.player?.state?.round_kills ?? "null");
                File.AppendAllText(path, "\r\nplayer.round_killhs " + data.player?.state?.round_killhs ?? "null");
                File.AppendAllText(path, "\r\nplayer.kills " + data.player?.match_stats?.kills ?? "null");
                File.AppendAllText(path, "\r\nplayer.assists " + data.player?.match_stats?.assists ?? "null");
                File.AppendAllText(path, "\r\nplayer.deaths " + data.player?.match_stats?.deaths ?? "null");
                File.AppendAllText(path, "\r\nplayer.mvps " + data.player?.match_stats?.mvps ?? "null");
                File.AppendAllText(path, "\r\nplayer.score " + data.player?.match_stats?.score ?? "null");
            }



            SentEvents();
        }

        private static void KillEvents(dynamic data)
        {
            var kills = Convert.ToInt32(data.player?.state?.round_kills);
            var hkills = Convert.ToInt32(data.player?.state?.round_killhs);
            switch (kills)
            {
                case 0:
                    break;
                case 1:
                    AddEvent(kills == hkills ? EventT.HKill : EventT.Kill);
                    break;
                case 2:
                    AddEvent(kills == hkills ? EventT.HDKill : EventT.DKill);
                    break;
                case 3:
                    AddEvent(kills == hkills ? EventT.HTKill : EventT.TKill);
                    break;
                case 4:
                    AddEvent(kills == hkills ? EventT.HFKill : EventT.FKill);
                    break;
                case 5:
                    AddEvent(kills == hkills ? EventT.HAce : EventT.Ace);
                    break;
            }
        }

        private static List<string> _events = new List<string>();

        private static void AddEvent(EventT gameEvent)
        {
            _events.Add(gameEvent.ToString());
        }

        private static void SentEvents()
        {
            var eventPipe = @"C:\Users\aisat\Documents\Visual Studio 2017\Projects\CSGOToTwitch\CSStatsForNerf\bin\Debug\netcoreapp2.1\events_pipe.txt";
            File.WriteAllText(eventPipe, JsonConvert.SerializeObject(_events));
            _events.Clear();
        }

        enum EventT
        {
            None,
            // kills
            Kill,
            DKill,
            TKill,
            FKill,
            Ace,
            // headshot kills
            HKill,
            HDKill,
            HTKill,
            HFKill,
            HAce,
            // Hp
            OneHp,
            // Money
            MaxMoney,
            NoMoney,
            // Game result
            Win,
            Lose,
            Draft,
            // Max stats - kills
            TenKills,
            TwentyKills,
            ThirtyKills,
            FortyKills,
            // Stats - k/d
            GodKD,
            UselessKD,
            UselessScore,
            // Super events
            Clutch,
            // Phases
            WarmUp,
            GameStart,
            RoundStart,
            Gameover,
            // Bomb
            TExploded,
            TDefused,
            TPlanted,
            CTExploded,
            CTDefused,
            CTPlanted
        }
    }
}
