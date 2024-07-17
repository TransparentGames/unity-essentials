using System;

namespace TransparentGames.Essentials.Data
{
    public interface IReadOnlyDataProperty<T>
    {
        public string Key { get; }
        public T Value { get; }
    }
}
