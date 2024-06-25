using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using UnityEngine.Pool;

namespace TransparentGames.Combat
{
    public class DamageIndicatorManager : MonoSingleton<DamageIndicatorManager>
    {
        [SerializeField] private DamageIndicator damageIndicatorPrefab;

        private ObjectPool<DamageIndicator> _pool;

        private void OnEnable()
        {
            _pool = new ObjectPool<DamageIndicator>(CreateDamageIndicator, GetDamageIndicator, ResetDamageIndicator, DestroyDamageIndicator, true, 10, 20);
        }

        private void OnDisable()
        {
            _pool.Clear();
        }

        public void Spawn(HitResult hitResult)
        {
            DamageIndicator damageIndicator = _pool.Get();
            damageIndicator.OnReturnToPool += Release;

            damageIndicator.Show(hitResult.damageDealt, hitResult.hitObject.transform.position);
        }

        public void Release(DamageIndicator damageIndicator)
        {
            damageIndicator.OnReturnToPool -= Release;
            _pool.Release(damageIndicator);
        }

        private DamageIndicator CreateDamageIndicator()
        {
            DamageIndicator damageIndicator = Instantiate(damageIndicatorPrefab, transform);

            return damageIndicator;
        }

        private void GetDamageIndicator(DamageIndicator damageIndicator)
        {
            damageIndicator.gameObject.SetActive(true);
        }

        private void ResetDamageIndicator(DamageIndicator damageIndicator)
        {
            damageIndicator.gameObject.SetActive(false);
        }

        private void DestroyDamageIndicator(DamageIndicator damageIndicator)
        {
            if (damageIndicator != null)
            {
                Destroy(damageIndicator.gameObject);
            }
        }
    }
}