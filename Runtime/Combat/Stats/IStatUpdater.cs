using System;
using System.Collections.Generic;

namespace TransparentGames.Stats
{
    public interface IStatUpdater
    {
        public event Action<List<Stat>> StatChanged;
    }
}