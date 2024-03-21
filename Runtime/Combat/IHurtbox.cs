using System;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    public interface IHurtbox
    {
        public event Action<HitInfo> Hit;

        public void OnHit(HitInfo hitInfo);
    }
}