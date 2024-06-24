using System;
using System.Collections.Generic;

namespace TransparentGames.Stats
{
    public interface IStatsRequired
    {
        public abstract void OnStatsChanged(List<Stat> stats);
    }
}
