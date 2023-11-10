using UnityEngine;
using Essentials.Abilities.ScriptableObjects;

namespace Essentials.Abilities
{
    //This class define what an ability is. It can varies drastically per game.
    public abstract class Ability : MonoBehaviour
    {
        public abstract bool CanUse(ICaster caster);
        public abstract void Cast(ICaster caster, SpecificMoment specificMoment = SpecificMoment.Windup);
    }
}