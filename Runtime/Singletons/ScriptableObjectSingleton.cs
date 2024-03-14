using System;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif


namespace TransparentGames.Core
{
    /// <summary>
    /// The basic ScriptableObject singleton implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject, ISingleton where T : ScriptableObjectSingleton<T>
    {
        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static T _instance;

        /// <summary>
        /// The initialization status of the singleton's instance.
        /// </summary>
        private SingletonInitializationStatus _initializationStatus = SingletonInitializationStatus.None;

        #endregion

        #region Properties

        public static void OnInitialized(Action<T> onInit)
        {
            if (InstanceExists)
            {
                onInit.Invoke(_instance);
            }
            else
            {
                _ = LoadAsync(Key, onInit);
            }
        }

        protected static string Key => typeof(T).Name;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load(Key);
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

        }

        protected static async Task LoadAsync(string key, System.Action<T> callback)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);

            if (handle.IsValid() == false) return;

            _instance = await handle.Task;

            callback(_instance);
        }

        protected static T Load(string key)
        {
            Debug.LogFormat("Loading... {0}", Key);
            return Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
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

#if UNITY_EDITOR
        protected virtual void OnEnable()
        {

            _ = CheckIfAddressable();
        }

        protected async Task CheckIfAddressable()
        {
            await Addressables.InitializeAsync().Task;
            var path = AssetDatabase.GetAssetPath(this);
            var guid = AssetDatabase.AssetPathToGUID(path);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, settings.DefaultGroup);

            if (entry.address != Key)
            {
                entry.address = Key;
            }
            else
            {
                Debug.LogFormat("Config {0} loaded successfully", this);
            }
        }
#endif
    }
}