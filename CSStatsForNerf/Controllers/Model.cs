﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StatsForNerf.ConsoleApp
{
    public class Provider
    {
        public string name { get; set; }
        public int appid { get; set; }
        public int version { get; set; }
        public string steamid { get; set; }
        public int timestamp { get; set; }
    }

    public class TeamCt
    {
        public int score { get; set; }
        public string name { get; set; }
        public int timeouts_remaining { get; set; }
        public int matches_won_this_series { get; set; }
    }

    public class TeamT
    {
        public int score { get; set; }
        public int timeouts_remaining { get; set; }
        public int matches_won_this_series { get; set; }
    }

    public class Map
    {
        public string mode { get; set; }
        public string name { get; set; }
        public string phase { get; set; }
        public int round { get; set; }
        public TeamCt team_ct { get; set; }
        public TeamT team_t { get; set; }
        public int num_matches_to_win_series { get; set; }
        public int current_spectators { get; set; }
        public int souvenirs_total { get; set; }
    }

    public class Round
    {
        public string phase { get; set; }
        public string bomb { get; set; }
        public string win_team { get; set; }
    }

    public class State
    {
        public int health { get; set; }
        public int armor { get; set; }
        public bool helmet { get; set; }
        public int flashed { get; set; }
        public int smoked { get; set; }
        public int burning { get; set; }
        public int money { get; set; }
        public int round_kills { get; set; }
        public int round_killhs { get; set; }
        public int equip_value { get; set; }
    }

    public class MatchStats
    {
        public int kills { get; set; }
        public int assists { get; set; }
        public int deaths { get; set; }
        public int mvps { get; set; }
        public int score { get; set; }
    }

    public class Player
    {
        public string steamid { get; set; }
        public string clan { get; set; }
        public string name { get; set; }
        public int observer_slot { get; set; }
        public string team { get; set; }
        public string activity { get; set; }
        public State state { get; set; }
        public MatchStats match_stats { get; set; }
    }

    public class Model
    {
        public Provider provider { get; set; }
        public Map map { get; set; }
        public Round round { get; set; }
        public Player player { get; set; }
    }
}
