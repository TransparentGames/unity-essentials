using UnityEngine;

namespace Project.Abilities.ScriptableObjects
{
    public abstract class EffectSO : DescriptionAbilityBaseSO
    {
        public SpecificMoment MomentToRun = SpecificMoment.Windup;

        public abstract void Activate(Ability ability, ICaster caster);
    }

    public enum SpecificMoment
    {
        Windup, Active, Recovery
    }
}