using System;
using System.Collections.Generic;

namespace TransparentGames.Essentials.Stats
{
    public interface IStatUpdater
    {
        public abstract Dictionary<string, float> CalculateStats(Dictionary<string, Stat> baseStats);
        public event Action StatChanged;
    }
}