using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.Singletons;
using TransparentGames.Essentials.UI;
using TransparentGames.Essentials.UI.ScreenSpace;
using UnityEngine;
using UnityEngine.Pool;

namespace TransparentGames.Essentials.Combat
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

        public void Spawn(HitResult hitResult, Vector3 offset)
        {
            DamageIndicator damageIndicator = _pool.Get();
            damageIndicator.transform.localPosition = new Vector3(damageIndicator.transform.localPosition.x, damageIndicator.transform.localPosition.y, 0);
            damageIndicator.transform.localScale = Vector3.one;

            var worldSpaceUIElement = damageIndicator.GetComponent<WorldSpaceUIElement>();
            worldSpaceUIElement.SetTarget(hitResult.hitObject.transform);
            worldSpaceUIElement.SetOffset(offset);

            damageIndicator.OnReturnToPool += Release;

            damageIndicator.Set(hitResult.damageDealt);
        }

        public void Release(DamageIndicator damageIndicator)
        {
            damageIndicator.OnReturnToPool -= Release;
            _pool.Release(damageIndicator);
        }

        private DamageIndicator CreateDamageIndicator()
        {
            DamageIndicator damageIndicator = Instantiate(damageIndicatorPrefab, DynamicElementsCanvas.Instance.GetTransform(), false);
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