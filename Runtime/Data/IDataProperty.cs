using System;

namespace TransparentGames.Data
{
    public interface IDataProperty<T> : IReadOnlyDataProperty<T>
    {
        public event Action Changed;
        public new T Value { get; set; }
        public void Save();
    }
}
