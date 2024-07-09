using System;
using TransparentGames.Essentials.UpdateManagement;
using UnityEngine;

namespace TransparentGames.Essentials.Time
{
    public class Timer
    {
        public float TimeLeft => _timeLeft;
        public event Action TimeIsUp;

        private float _timeLeft;
        private bool _isRunning;
        private IUpdateEntity _updateEntity;

        public Timer(float time)
        {
            _timeLeft = time;
        }

        public void Start()
        {
            _isRunning = true;
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
                _timeLeft -= Time.deltaTime;
                if (_timeLeft <= 0)
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