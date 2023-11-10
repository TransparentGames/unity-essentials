using UnityEngine;

namespace Project.Abilities.ScriptableObjects
{
    public abstract class RequirementSO : ScriptableObject
    {
        public abstract bool Execute(ICaster caster);
    }
}