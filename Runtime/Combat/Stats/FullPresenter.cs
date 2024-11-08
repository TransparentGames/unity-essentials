using TransparentGames.Essentials.Combat;
using TransparentGames.Essentials.UI;
using TransparentGames.Essentials.UI.ScreenSpace;
using TransparentGames.Essentials.UI.WorldSpace;
using UnityEngine;

namespace TransparentGames.Essentials.Stats
{
    public class FullPresenter : ComponentBase
    {
        [SerializeField] private Bar healthBarPrefab;
        [SerializeField] private bool isWorldSpace = true;
        [SerializeField] private Vector3 worldOffset;

        private IHealth _health;
        private Bar _healthBar;
        private ILevelable _levelable;

        private void Awake()
        {
            _health = owner.GetComponentInChildren<IHealth>();
            _levelable = owner.GetComponentInChildren<ILevelable>();

            if (isWorldSpace)
                CreateWorldSpaceHealthBar();
            else
                DynamicElementsCanvas.Initialized(() => CreateScreenSpaceHealthBar());

            _health.ValueChanged += OnHealthChanged;
            _health.ValueInitialized += OnInitialized;
            _levelable.LevelChanged += OnLevelChanged;
        }

        private void OnEnable()
        {
            OnInitialized();
            OnLevelChanged(_levelable.Level);
        }

        private void OnInitialized()
        {
            if (_healthBar == null)
                return;

            if (isWorldSpace)
            {
                _healthBar.transform.position = transform.position + worldOffset;
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
            _healthBar = Instantiate(healthBarPrefab, transform, false);

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
                _healthBar.UpdateValue(currentHealth);
            }
        }

        private void OnLevelChanged(int newLevel)
        {
            if (_healthBar != null)
            {
                _healthBar.SetLevel(newLevel);
            }
        }
    }
}