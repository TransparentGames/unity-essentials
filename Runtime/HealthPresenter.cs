using System;
using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.UI;
using TransparentGames.Essentials.UI.ScreenSpace;
using TransparentGames.Essentials.UI.WorldSpace;
using UnityEngine;

namespace TransparentGames.Essentials.Combat
{
    [RequireComponent(typeof(IHealth))]
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private Bar healthBarPrefab;
        [SerializeField] private Vector3 healthBarOffset;

        private IHealth _health;
        private Bar _healthBar;

        private void Awake()
        {
            _health = GetComponent<IHealth>();

            DynamicElementsCanvas.Initialized(() => CreateScreenSpaceHealthBar());

            _health.ValueChanged += OnHealthChanged;
            _health.ValueInitialized += OnInitialized;
        }

        private void OnEnable()
        {
            OnInitialized();
        }

        private void OnInitialized()
        {
            if (_healthBar == null)
                return;

            _healthBar.Set(_health.MaxHealth, _health.CurrentHealth);
            _healthBar.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (_healthBar == null)
                return;

            _healthBar.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _health.ValueInitialized -= OnInitialized;
            _health.ValueChanged -= OnHealthChanged;
        }

        private void CreateScreenSpaceHealthBar()
        {
            _healthBar = Instantiate(healthBarPrefab, DynamicElementsCanvas.Instance.GetTransform(), false);

            _healthBar.Set(_health.MaxHealth, _health.CurrentHealth);
            _healthBar.gameObject.SetActive(gameObject.activeSelf);
        }

        private void OnHealthChanged(float currentHealth)
        {
            if (_healthBar != null)
            {
                _healthBar.UpdateValue(currentHealth);
            }
        }
    }
}