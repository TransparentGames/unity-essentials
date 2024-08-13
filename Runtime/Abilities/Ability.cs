using System;
using System.Collections.Generic;
using UnityEngine;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Stats;

namespace TransparentGames.Essentials.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public AbilityTemplate AbilityTemplate;
        public bool CanCancel { get; set; } = true;
        public bool CanHardCancel { get; set; } = false;
        public event Action<List<HitResult>> HitResultsEvent;
        public event Action Finished;

        public virtual bool CanUse(Caster caster)
        {
            var abilityComponents = GetComponentsInChildren<IAbilityComponent>();
            var canUse = true;
            foreach (var abilityComponent in abilityComponents)
            {
                canUse &= abilityComponent.CanUse(caster);
            }
            return canUse;
        }

        public abstract void Use(Caster caster);
        public abstract void Cancel();

        protected virtual void OnUsed(Caster caster)
        {
            var abilityComponents = GetComponentsInChildren<IAbilityComponent>();
            foreach (var abilityComponent in abilityComponents)
            {
                abilityComponent.Use(caster);
            }
        }

        protected virtual void OnFinished()
        {
            Finished?.Invoke();
            Finished = null;
        }

        protected void OnHitResults(List<HitResult> hitResults)
        {
            HitResultsEvent?.Invoke(hitResults);
        }
    }
}