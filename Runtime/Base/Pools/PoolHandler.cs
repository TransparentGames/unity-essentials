using UnityEngine;
using UnityEngine.Pool;

namespace TransparentGames.Essentials.Pools
{
    public class PoolHandler<T> where T : Component
    {
        private readonly ObjectPool<T> _pool;
        private readonly T _componentTemplate;
        private readonly Transform _parent;

        public PoolHandler(T componentTemplate, Transform parent, int defaultCapacity = 10, int maxSize = 20)
        {
            _componentTemplate = componentTemplate;
            _parent = parent;

            _pool = new ObjectPool<T>(
                CreateComponent,
                OnGetComponent,
                OnReleaseComponent,
                OnDestroyComponent,
                true,
                defaultCapacity,
                maxSize
            );
        }

        private T CreateComponent()
        {
            var component = Object.Instantiate(_componentTemplate, _parent);
            component.transform.position = Vector3.zero;
            component.gameObject.SetActive(false);
            return component;
        }

        private void OnGetComponent(T component)
        {
            component.gameObject.SetActive(true);
        }

        private void OnReleaseComponent(T component)
        {
            component.gameObject.SetActive(false);
        }

        private void OnDestroyComponent(T component)
        {
            Object.Destroy(component.gameObject);
        }

        public T Get()
        {
            return _pool.Get();
        }

        public void Release(T component)
        {
            _pool.Release(component);
        }

        public void Dispose()
        {
            _pool.Dispose();
        }
    }
}
