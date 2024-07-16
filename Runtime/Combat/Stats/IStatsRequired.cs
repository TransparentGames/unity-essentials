using System;
using System.Collections.Generic;

namespace TransparentGames.Essentials.Stats
{
    public interface IStatsRequired
    {
        public void OnStatsChanged(StatsHolder statsHolder);
    }
}
