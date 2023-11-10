using System;
using Project.Abilities.ScriptableObjects;
using UnityEngine;

namespace Project.Abilities
{
    //Enable you to have diverse type of object to cast an ability. By example, a turret or a trap.
    public interface ICaster
    {
        public bool IsCasting { get; }
        public Animator Animator { get; }

        public void BasicAttack();

        public void Ability1()
        {
            throw new NotImplementedException();
        }

        public void Ability2()
        {
            throw new NotImplementedException();
        }

        public bool CanCast(AbilityTypes abilityType);

        public void EquipAbility(AbilityDefinitionSO definition);

        public IStat RetrieveStats(StatDefinitionSO stat);

        //public void ShootProjectile(ProjectileDefinitionSO projectile);

        //public void RegisterOnChannelEnd(System.Action<bool> callback);
        //public void UnregisterOnChannelEnd(System.Action<bool> callback);
    }

    public enum AbilityTypes
    {
        BasicAttack,
        Ability1,
        Ability2,
    }
}