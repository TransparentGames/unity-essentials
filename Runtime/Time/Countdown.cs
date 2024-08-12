using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using TransparentGames.Essentials.UpdateManagement;

namespace TransparentGames.Essentials.Time
{
    public class Countdown
    {
        public float TimeLeft => _timeLeft;
        public float TimeStart => _timeStart;
        public event Action<Countdown> TimeIsUp;

        private float _timeLeft;
        private float _timeStart;
        private bool _isRunning;
        private IUpdateEntity _updateEntity;

        public Countdown(float time)
        {
            _timeLeft = time;
            _timeStart = time;
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
                _timeLeft -= UnityEngine.Time.deltaTime;
                if (_timeLeft <= 0)
                {
                    TimeIsUp?.Invoke(this);
                    TimeIsUp = null;

                    _isRunning = false;
                    UpdateManager.Stop(_updateEntity);
                }
            }
        }
    }
}
