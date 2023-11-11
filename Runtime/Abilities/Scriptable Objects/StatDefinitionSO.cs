
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Essentials.Abilities.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Stat Definition", menuName = "Abilities/Stat Definition", order = 1)]
    public class StatDefinitionSO : ScriptableObject
    {
        public string StatName;
        public Image Icon;
        public string Description;
    }
}