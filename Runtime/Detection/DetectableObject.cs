using System;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public class DetectableObject : MonoBehaviour, IDetectable, IComponent
    {
        public GameObject Owner { get; set; }
        public bool IsDetectable => gameObject.activeSelf;
        public event Action<IDetectable> DetectionChanged;

        protected void OnEnable()
        {
            DetectableRegistry.Instance.Register(this);
            OnDetectionChanged();
        }

        protected void OnDisable()
        {
            DetectableRegistry.UnRegister(this);
            OnDetectionChanged();
        }

        protected void OnDetectionChanged()
        {
            DetectionChanged?.Invoke(this);
        }
    }
}