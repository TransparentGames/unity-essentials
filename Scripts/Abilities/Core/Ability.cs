using UnityEngine;
using Project.Abilities.ScriptableObjects;

namespace Project.Abilities
{
    //This class define what an ability is. It can varies drastically per game.
    public abstract class Ability : MonoBehaviour
    {
        public abstract bool CanUse(ICaster caster);
        public abstract void Cast(ICaster caster, SpecificMoment specificMoment = SpecificMoment.Windup);
    }
}