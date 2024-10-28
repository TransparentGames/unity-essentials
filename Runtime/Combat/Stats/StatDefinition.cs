using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [CreateAssetMenu(fileName = "Stat Definition", menuName = "Transparent Games/Stats/Stat Definition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        public string Type => statId;
        public virtual string FamilyType => statId;

        [SerializeField] private string statId;
        [SerializeField] private string statName;
        [SerializeField] private float displayMultiplier = 1f;
        [SerializeField] private string displayFormat = "F2";
        [SerializeField] private string suffix = "";

        public string GetDisplayValue(float value)
        {
            return (value * displayMultiplier).ToString(displayFormat) + suffix;
        }

        public string GetDisplayName()
        {
            return statName;
        }

        public virtual float Calculate(float statValue, float baseValue)
        {
            return statValue;
        }
    }
}