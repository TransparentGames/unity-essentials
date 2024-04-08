using System;
using UnityEngine.Assertions;

namespace TransparentGames.Essentials.UpdateManagement
{
    [Serializable]
    public class UpdateEntity : IUpdateEntity
    {
        private readonly Action _action;
        private readonly int _interval;

        private int _counter;
        public UpdateEntity(Action action, int interval)
        {
            interval = Math.Max(1, interval);
            Assert.IsNotNull(action);
            _action = action;
            _interval = interval;
        }
        public void Tick()
        {
            if (_counter++ % _interval == 0)
            {
                _action?.Invoke();
                _counter = 1;
            }
        }
    }
}
