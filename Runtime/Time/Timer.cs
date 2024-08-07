using System;
using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.Time
{
    public class Timer
    {
        public float TimeElapsed => _timeElapsed;
        public event Action TimeIsUp;

        private float _timeElapsed;
        private float _timeLimit;
        private bool _isRunning;
        private IUpdateEntity _updateEntity;

        public Timer(float time)
        {
            _timeLimit = time;
        }

        public void Start()
        {
            _isRunning = true;
            _timeElapsed = 0f;
            _updateEntity = UpdateManager.StartUpdate(Update, UpdateType.Update);
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Update()
        {
            if (_isRunning)
            {
                _timeElapsed += UnityEngine.Time.deltaTime;
                if (_timeElapsed >= _timeLimit)
                {
                    TimeIsUp?.Invoke();
                    TimeIsUp = null;

                    _isRunning = false;
                    UpdateManager.Stop(_updateEntity);
                }
            }
        }
    }
}