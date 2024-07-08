using System;
using System.Collections.Generic;

namespace TransparentGames.Stats
{
    public interface IStatsRequired
    {
        public void OnStatsChanged(StatsHolder statsHolder);
    }
}
