using UnityEngine;

namespace TransparentGames.Stats
{
    [CreateAssetMenu(fileName = "Stat Definition", menuName = "Transparent Games/Stats/Stat Definition", order = 0)]
    public class StatDefinition : ScriptableObject
    {
        public string statName;
    }
}