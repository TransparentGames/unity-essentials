using System;

namespace TransparentGames.Essentials.Stats
{
    public interface ILevelable
    {
        public event Action<int> LevelChanged;
        public int Level { get; set; }
    }
}