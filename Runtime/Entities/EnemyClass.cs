using UnityEngine;

namespace TransparentGames.Essentials.Entities
{
    [CreateAssetMenu(fileName = "New Enemy Class", menuName = "Transparent Games/Design/Enemy Class", order = 0)]
    public class EnemyClass : ScriptableObject
    {
        public string className;
    }
}