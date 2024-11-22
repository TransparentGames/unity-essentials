using System;
using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.Time
{
    public class Countdown : ITimeTracker
    {
        public float DefaultTime => _defaultTime;
        public float CurrentTime => _timeLeft;
        public event Action<Countdown> TimeIsUp;

        private float _timeLeft;
        private float _defaultTime;
        private bool _isRunning;
        private IUpdateEntity _updateEntity;

        public Countdown(float time)
        {
            _timeLeft = time;
            _defaultTime = time;
        }

        public void Start()
        {
            _isRunning = true;
            if (_updateEntity != null)
                UpdateManager.Stop(_updateEntity);

            _updateEntity = UpdateManager.StartUpdate(Update, UpdateType.Update);
        }

        public void Reset()
        {
            _timeLeft = _defaultTime;
        }

        public void Reset(float time)
        {
            _timeLeft = time;
            _defaultTime = time;
        }

        public void Stop()
        {
            _isRunning = false;
            if (_updateEntity != null)
                UpdateManager.Stop(_updateEntity);
        }

        public void Update()
        {
            if (_isRunning)
            {
                _timeLeft -= UnityEngine.Time.deltaTime;
                if (_timeLeft <= 0)
                {
                    TimeIsUp?.Invoke(this);

                    Stop();
                }
            }
        }
    }
}