using UnityEngine;
using UnityEngine.UI;

namespace Project.Abilities.ScriptableObjects
{
    //Define what an abilities is
    public abstract class AbilityDefinitionSO : ScriptableObject
    {
        [SerializeField] private Image icon;
        [SerializeField] private string description;

        public abstract Ability Instantiate(ICaster caster);
    }
}