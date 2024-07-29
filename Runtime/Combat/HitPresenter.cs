using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    [RequireComponent(typeof(IHittable))]
    public class HitPresenter : MonoBehaviour
    {
        [SerializeField] private Vector3 damageIndicatorOffset;
        [SerializeField] private HitTextColorTemplate hitTextColorTemplate;
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
            DamageIndicatorManager.Initialized(() => DamageIndicatorManager.Instance.Spawn(hitResult, damageIndicatorOffset, hitTextColorTemplate));
        }
    }
}