using System;

namespace Essentials.Abilities
{
    [Serializable]
    public abstract class Stat<T> : IStat where T : struct
    {
        public abstract string Name { get; }
        public abstract T Value { get; set; }
    }
}