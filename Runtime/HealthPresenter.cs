using System;
using TransparentGames.Essentials.Combat;
using TransparentGames.UI;
using TransparentGames.UI.ScreenSpace;
using TransparentGames.UI.WorldSpace;
using UnityEngine;

namespace TransparentGames.Combat
{
    [RequireComponent(typeof(IHealth))]
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBarPrefab;
        [SerializeField] private Vector3 healthBarOffset;
        [SerializeField] private bool isWorldSpace = true;

        private IHealth _health;
        private HealthBar _healthBar;

        private void Awake()
        {
            _health = GetComponent<IHealth>();

            if (isWorldSpace)
                WorldSpaceCanvas.Initialized(() => CreateWorldSpaceHealthBar());
            else
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

            if (isWorldSpace)
            {
                var worldSpaceElement = _healthBar.GetComponent<WorldSpaceElement>();
                worldSpaceElement.SetOffset(healthBarOffset);
                worldSpaceElement.SetTarget(transform);
            }

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

        private void CreateWorldSpaceHealthBar()
        {
            _healthBar = Instantiate(healthBarPrefab, WorldSpaceCanvas.Instance.GetTransform(), false);
            _healthBar.transform.localPosition = new Vector3(_healthBar.transform.localPosition.x, _healthBar.transform.localPosition.y, 0);
            _healthBar.transform.localScale = Vector3.one;

            _healthBar.Set(_health.MaxHealth, _health.CurrentHealth);
            _healthBar.gameObject.SetActive(gameObject.activeSelf);
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
                _healthBar.UpdateHealth(currentHealth);
            }
        }
    }
}