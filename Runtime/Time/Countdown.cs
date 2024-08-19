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

        private float _timeLeft = 0;
        private float _timeStart = 0;
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

        public void Reset(float time)
        {
            _timeLeft = time;
            _timeStart = time;
        }

        public void Stop()
        {
            _isRunning = false;
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

                    _isRunning = false;
                    UpdateManager.Stop(_updateEntity);
                }
            }
        }
    }
}
