using TransparentGames.Combat;
using TransparentGames.Essentials.Combat;
using UnityEngine;

namespace TransparentGames.Combat
{
    [RequireComponent(typeof(IHittable))]
    public class HitPresenter : MonoBehaviour
    {
        [SerializeField] private Vector3 damageIndicatorOffset;

        private IHittable _hittable;

        private void Awake()
        {
            _hittable = GetComponent<IHittable>();
        }

        private void OnEnable()
        {
            _hittable.HitResultEvent += OnHit;
        }

        private void OnDisable()
        {
            _hittable.HitResultEvent -= OnHit;
        }

        private void OnHit(HitResult hitResult)
        {
            DamageIndicatorManager.Initialized(() => DamageIndicatorManager.Instance.Spawn(hitResult, damageIndicatorOffset));
        }
    }
}