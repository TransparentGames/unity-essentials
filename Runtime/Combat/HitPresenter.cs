using TransparentGames.Combat;
using TransparentGames.Essentials.Combat;
using UnityEngine;

namespace TransparentGames.Combat
{
    [RequireComponent(typeof(IHittable))]
    public class HitPresenter : MonoBehaviour
    {
        private IHittable _hittable;

        private void Awake()
        {
            _hittable = GetComponent<IHittable>();
            _hittable.HitResultEvent += OnHit;
        }

        private void OnDestroy()
        {
            _hittable.HitResultEvent -= OnHit;
        }

        private void OnHit(HitResult hitResult)
        {
            if (DamageIndicatorManager.InstanceExists)
                DamageIndicatorManager.Instance.Spawn(hitResult);
        }
    }
}