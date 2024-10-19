using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TransparentGames.Essentials.Singletons
{
    /// <summary>
    /// The basic MonoBehaviour singleton implementation, this singleton is destroyed after scene changes, use <see cref="PersistentMonoSingleton{T}"/> if you want a persistent and global singleton instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        protected static T _instance;
        private static Action _initialized;

        /// <summary>
        /// The initialization status of the singleton's instance.
        /// </summary>
        private SingletonInitializationStatus _initializationStatus = SingletonInitializationStatus.None;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static void Initialized(Action onInitializeCallback)
        {
            if (InstanceExists)
            {
                onInitializeCallback?.Invoke();
                return;
            }

            _initialized += onInitializeCallback;
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"No instance of {typeof(T).Name} found in the scene, do you perhaps wanted to create a new one?");
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets whether the singleton's instance is initialized.
        /// </summary>
        public virtual bool IsInitialized => _initializationStatus == SingletonInitializationStatus.Initialized;

        public static bool InstanceExists => _instance != null;

        #endregion

        #region Unity Messages

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                // Initialize existing instance
                InitializeSingleton();
            }
            else
            {

                // Destroy duplicates
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This gets called once the singleton's instance is created.
        /// </summary>
        protected virtual void OnMonoSingletonCreated()
        {

        }

        protected virtual void OnInitializing()
        {

        }

        protected virtual void OnInitialized()
        {
            _initialized?.Invoke();
            _initialized = null;
        }

        #endregion

        #region Public Methods

        public virtual void InitializeSingleton()
        {
            if (_initializationStatus != SingletonInitializationStatus.None)
            {
                return;
            }

            _initializationStatus = SingletonInitializationStatus.Initializing;
            OnInitializing();
            _initializationStatus = SingletonInitializationStatus.Initialized;
            OnInitialized();
        }

        public virtual void ClearSingleton() { }

        public static void CreateInstance()
        {
            DestroyInstance();
            _instance = Instance;
        }

        public static void DestroyInstance()
        {
            if (_instance == null)
            {
                return;
            }

            _instance.ClearSingleton();
            _instance = default;
        }

        #endregion
    }
}