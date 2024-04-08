using System;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public class DetectableObject : MonoBehaviour, IDetectable, IComponent
    {
        public GameObject Owner { get; set; }
        public bool IsDetectable => gameObject.activeSelf;
        public event Action<IDetectable> DetectionChanged;
        private void OnEnable()
        {
            DetectableRegistry.Instance.Register(this);
            DetectionChanged?.Invoke(this);
        }

        private void OnDisable()
        {
            DetectableRegistry.UnRegister(this);
            DetectionChanged?.Invoke(this);
        }
    }
}