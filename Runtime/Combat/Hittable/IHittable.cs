using System;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public interface IHittable
    {
        public Entity Owner { get; }
        public Transform Transform { get; }
        public event Action<HitResult> HitResultEvent;
        public event Action<HitInfo> HitInfoEvent;

        public HitResult OnHit(HitInfo info);
    }
}