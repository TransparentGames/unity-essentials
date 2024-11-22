using System;

namespace TransparentGames.Essentials.Time
{
    public interface ITimeTracker
    {
        public float DefaultTime { get; }
        public float CurrentTime { get; }

        void Start();
        void Stop();
        void Reset();
    }
}