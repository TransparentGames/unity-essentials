using System;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public interface IHittable
    {
        public GameObject GameObject { get; }
        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        public HitResult OnHit(HitInfo info);
    }
}