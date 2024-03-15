using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TransparentGames.Essentials
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private GameSceneSO persistentManagerSceneSO = default;
        [SerializeField] private GameSceneSO mainMenuSceneSO = default;

#if !UNITY_SERVER
        private void Start()
        {
            persistentManagerSceneSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnPersistentManagerSceneLoaded;
        }
#endif

        private void OnPersistentManagerSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            SceneLoader.Instance.LoadMenu(mainMenuSceneSO);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}