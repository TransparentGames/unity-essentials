using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    public interface IDetector
    {
        public event Action<IDetectable> ObjectDetected;
        public event Action<IDetectable> ObjectLostDetection;

        public Entity Owner { get; }
        public bool IsAnyDetected { get; }
        public IReadOnlyList<IDetectable> AllDetected { get; }
        public IDetectable GetClosest();
        public void Clear();
    }
}