using System;
using System.Collections.Generic;
using UnityEngine;
using TransparentGames.Essentials.Combat;

namespace TransparentGames.Essentials.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public event Action<List<HitResult>> HitResultsEvent;
        public event Action Finished;
        public HitInfo HitInfo { get; set; }
        public LayerMask LayerMask { get; set; }

        public abstract void Use(Caster caster);

        protected void OnFinished()
        {
            Finished?.Invoke();
        }

        protected void OnHitResults(List<HitResult> hitResults)
        {
            HitResultsEvent?.Invoke(hitResults);
        }
    }
}