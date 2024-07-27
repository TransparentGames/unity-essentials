
using System;
using UnityEngine;

namespace TransparentGames.Essentials.Abilities
{
    public abstract class Caster : MonoBehaviour, IComponent
    {
        public virtual event Action Ready;
        public GameObject Owner { get; set; }

        public abstract Animator Animator { get; }
        public abstract bool IsBusy { get; }
        public abstract bool CanCancel { get; }
        public abstract bool CanHardCancel { get; }

        public abstract void Cast(GameObject target = null);
        public abstract bool CanCast();
        public abstract void Cancel();

        protected void OnReady()
        {
            Ready?.Invoke();
        }
    }
}