using System;
using System.Collections.Generic;

namespace TransparentGames.Stats
{
    public interface IStatsRequired
    {
        public StatsHolder StatsHolder { get; set; }
        public void OnStatsChanged();
    }
}
