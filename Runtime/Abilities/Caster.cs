
using System;
using UnityEngine;

namespace TransparentGames.Essentials.Abilities
{
    public abstract class Caster : MonoBehaviour, IComponent
    {
        public virtual event Action Ready;
        public GameObject Owner { get; set; }

        public abstract Animator Animator { get; }
        public abstract bool IsReady { get; }

        public abstract void Cast(GameObject target = null);
        public abstract bool CanCast();

        protected void OnReady()
        {
            Ready?.Invoke();
        }
    }
}