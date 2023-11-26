using UnityEngine;
using System.Text.RegularExpressions;

public class PersistentMonoSingleton<T> : MonoSingleton<MonoBehaviour> where T : MonoBehaviour
{
    public static new T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new()
                    {
                        name = Regex.Replace(typeof(T).Name, "(\\B[A-Z])", " $1")
                    };
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private static T _instance;
}
