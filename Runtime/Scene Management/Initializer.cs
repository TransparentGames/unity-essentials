using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TransparentGames.Essentials.SceneManagement
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private GameSceneSO persistentManagerSceneSO = default;
        [SerializeField] private GameSceneSO mainMenuSceneSO = default;

#if !UNITY_SERVER
        private async void Start()
        {
            // Load the persistent manager scene asynchronously
            AsyncOperationHandle<SceneInstance> persistentManagerSceneHandle = Addressables.LoadSceneAsync(persistentManagerSceneSO.sceneReference, LoadSceneMode.Additive);

            // Wait until the persistent manager scene is fully loaded
            await persistentManagerSceneHandle.Task;

            // Once loaded, load the main menu scene
            SceneLoader.Instance.LoadMenu(mainMenuSceneSO);

            // Unload the current scene
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
#endif
    }
}