using TransparentGames.Entities;
using UnityEngine;

namespace TransparentGames.Essentials.Entities
{
    [CreateAssetMenu(fileName = "Enemy Template", menuName = "Transparent Games/Enemies/Enemy Template", order = 0)]
    public class EnemyTemplate : EntityTemplate
    {
        public EnemyClass enemyClass;
    }
}