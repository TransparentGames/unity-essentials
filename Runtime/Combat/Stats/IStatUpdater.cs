using System;
using System.Collections.Generic;

namespace TransparentGames.Stats
{
    public interface IStatUpdater
    {
        public abstract List<Stat> CalculateStats(List<Stat> baseStats);
        public event Action StatChanged;
    }
}