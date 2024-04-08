using System;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public interface IDetectable
    {
        public GameObject Owner { get; }

        public bool IsDetectable { get; }
        public event Action<IDetectable> DetectionChanged;
        public bool CompareTag(string tag);
    }
}