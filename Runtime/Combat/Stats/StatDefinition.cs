using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    [CreateAssetMenu(fileName = "Stat Definition", menuName = "Transparent Games/Stats/Stat Definition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        public string statName;

        [SerializeField] private float displayMultiplier = 1f;
        [SerializeField] private string displayFormat = "F2";
        [SerializeField] private string suffix = "";

        public string GetDisplayValue(float value)
        {
            return (value * displayMultiplier).ToString(displayFormat) + suffix;
        }
    }
}