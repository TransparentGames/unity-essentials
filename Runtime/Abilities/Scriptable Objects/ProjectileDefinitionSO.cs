using UnityEngine;

namespace Essentials.Abilities.ScriptableObjects
{
    public class ProjectileDefinitionSO : ScriptableObject
    {
        [SerializeField] private GameObject abilityPrefab; //[FireBallProjectile]

        public GameObject Instantiate()
        {
            return Instantiate(abilityPrefab);
        }
    }
}