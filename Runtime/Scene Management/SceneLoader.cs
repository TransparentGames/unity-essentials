using System;
using System.Collections;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TransparentGames.Essentials.SceneManagement
{
    /// <summary>
    /// This class manages the scene loading and unloading.
    /// </summary>
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        public Action SceneReady;
        public bool IsLoading => _isLoading;
        public Action<bool> ToggleLoadingScreen;

        [SerializeField] private GameSceneSO gameplayScene = default;

        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;
        //Parameters coming from scene loading requests
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;
        private SceneInstance _gameplayManagerSceneInstance = new();
        private float _fadeDuration = .5f;
        private bool _isLoading = false; //To prevent a new loading request while already loading a new scene

#if UNITY_EDITOR
        /// <summary>
        /// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialization.
        /// </summary>
        public void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            _currentlyLoadedScene = currentlyOpenedLocation;

            if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
            {
                // Load the gameplay scene asynchronously
                _gameplayManagerLoadingOpHandle = Addressables.LoadSceneAsync(gameplayScene.sceneReference, LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplaySceneCompleted;
            }
        }
#endif

        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        public void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            if (_gameplayManagerSceneInstance.Scene == null
                || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle = Addressables.LoadSceneAsync(gameplayScene.sceneReference, LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        /// <summary>
        /// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
        /// </summary>
        public void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (_isLoading)
                return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            if (_gameplayManagerSceneInstance.Scene != null
                && _gameplayManagerSceneInstance.Scene.isLoaded)
            {
                Addressables.Release(_gameplayManagerLoadingOpHandle);
            }

            StartCoroutine(UnloadPreviousScene());
        }

        private void OnGameplaySceneCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _gameplayManagerSceneInstance = obj.Result;
                StartGameplay();
            }
            else
            {
                Debug.LogError($"Failed to load gameplay scene: {obj.OperationException}");
            }
        }

        private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _gameplayManagerSceneInstance = obj.Result;
                StartCoroutine(UnloadPreviousScene());
            }
            else
            {
                Debug.LogError($"Failed to load gameplay managers: {obj.OperationException}");
            }
        }

        /// <summary>
        /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
        /// </summary>
        private IEnumerator UnloadPreviousScene()
        {
            yield return new WaitForSeconds(_fadeDuration);

            if (_currentlyLoadedScene != null) //would be null if the game was started in Initialization
            {
                if (_loadingOperationHandle.IsValid())
                {
                    //Unload the scene through the Addressable system
                    Addressables.UnloadSceneAsync(_loadingOperationHandle);
                }
#if UNITY_EDITOR
                else
                {
                    //Only used when, after a "cold start", the player moves to a new scene
                    //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                    //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                    SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            LoadNewScene();
        }

        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        private void LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                ToggleLoadingScreen?.Invoke(true);
            }

            _loadingOperationHandle = Addressables.LoadSceneAsync(_sceneToLoad.sceneReference, LoadSceneMode.Additive, true);
            _loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            //Save loaded scenes (to be unloaded at next load request)
            _currentlyLoadedScene = _sceneToLoad;

            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);
            LightProbes.TetrahedralizeAsync();

            _isLoading = false;

            if (_showLoadingScreen)
                ToggleLoadingScreen?.Invoke(false);

            StartGameplay();
        }

        private void StartGameplay()
        {
            SceneReady?.Invoke();
        }

        private void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}