using System;
using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.Time
{
    public class Timer : ITimeTracker
    {
        public float DefaultTime => 0f;
        public float CurrentTime => _timeElapsed;

        private float _timeElapsed;
        private bool _isRunning;
        private IUpdateEntity _updateEntity;

        public Timer()
        {
            _timeElapsed = 0f;
        }

        public void Start()
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("Timer is already running.");
            }

            _isRunning = true;
            _updateEntity = UpdateManager.StartUpdate(Update, UpdateType.Update);
        }

        public void Stop()
        {
            if (!_isRunning)
            {
                return;
            }

            _isRunning = false;
            UpdateManager.Stop(_updateEntity);
        }

        public void Update()
        {
            if (_isRunning)
            {
                _timeElapsed += UnityEngine.Time.deltaTime;
            }
        }

        public void Reset()
        {
            _timeElapsed = 0f;
        }
    }
}