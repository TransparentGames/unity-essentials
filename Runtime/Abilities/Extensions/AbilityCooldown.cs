using System.Collections;
using UnityEngine;

namespace TransparentGames.Abilities
{
    public class AbilityCooldown : MonoBehaviour
    {
        public bool IsReady => _isReady;

        private float _cooldownTime;
        private float _nextReadyTime;
        private bool _isReady = true;

        public void StartTimer(float cooldown)
        {
            _cooldownTime = cooldown;
            _nextReadyTime = Time.time + _cooldownTime;
            StartCoroutine(nameof(Cooldown));
        }

        private IEnumerator Cooldown()
        {
            _isReady = false;
            while (Time.time < _nextReadyTime)
            {
                yield return null;
            }
            _isReady = true;
        }
    }
}