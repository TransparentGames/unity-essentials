using System;

namespace TransparentGames.Data
{
    public interface IReadOnlyDataProperty<T>
    {
        public string Key { get; }
        public T Value { get; }
    }
}
