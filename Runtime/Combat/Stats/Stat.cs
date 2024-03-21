using System;

namespace TransparentGames.Combat
{
    [Serializable]
    public abstract class Stat<T> : IStat where T : struct
    {
        public abstract string Name { get; }
        public abstract T Value { get; set; }
    }
}