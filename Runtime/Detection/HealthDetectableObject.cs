using System;
using TransparentGames.Essentials.Combat;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    [RequireComponent(typeof(IHealth))]
    public class HealthDetectableObject : DetectableObject
    {
        private void Awake()
        {
            GetComponent<IHealth>().ValueZeroed += OnHealthValueZeroed;
        }

        private void OnDestroy()
        {
            GetComponent<IHealth>().ValueZeroed -= OnHealthValueZeroed;
        }

        private void OnHealthValueZeroed()
        {
            DetectableRegistry.UnRegister(this);
            OnDetectionChanged();
        }
    }
}