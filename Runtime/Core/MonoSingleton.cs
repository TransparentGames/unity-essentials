using UnityEngine;

namespace Singletons
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }
        public static bool IsInitialized => _instance != null;

        private static T _instance;
    }
}