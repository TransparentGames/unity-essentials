using System.Text.RegularExpressions;
using UnityEngine;

namespace TransparentGames.Essentials.Singletons
{
    /// <summary>
    /// This singleton is persistent across scenes by calling <see cref="Object.DontDestroyOnLoad(Object)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PersistentMonoSingleton<T> : MonoSingleton<T> where T : PersistentMonoSingleton<T>
    {
        public new static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene, creating a new one.");
                        GameObject obj = new()
                        {
                            name = Regex.Replace(typeof(T).Name, "(\\B[A-Z])", " $1")
                        };
                        _instance = obj.AddComponent<T>();
                        _instance.OnMonoSingletonCreated();
                    }
                }
                return _instance;
            }
        }

        #region Protected Methods

        protected override void OnInitializing()
        {
            base.OnInitializing();
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion
    }
}