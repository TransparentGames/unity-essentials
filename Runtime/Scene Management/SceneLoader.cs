using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TransparentGames.Essentials
{
    /// <summary>
    /// This class manages the scene loading and unloading.
    /// </summary>
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        public Action SceneReady;
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
                gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnGameplaySceneCompleted;
            }
        }
#endif

        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        public void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            if (_gameplayManagerSceneInstance.Scene == null
                || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle = gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
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
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
            if (_gameplayManagerSceneInstance.Scene != null
                && _gameplayManagerSceneInstance.Scene.isLoaded)
                Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);

            StartCoroutine(UnloadPreviousScene());
        }

        private void OnGameplaySceneCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            // This callback will be executed when the scene loading is completed
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                // Get the loaded scene instance
                _gameplayManagerSceneInstance = obj.Result;

                // Start the gameplay after the scene is loaded
                StartGameplay();
            }
            else
            {
                // Handle failure if necessary
                Debug.LogError("Failed to load gameplay scene.");
            }
        }

        private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

            StartCoroutine(UnloadPreviousScene());
        }

        /// <summary>
        /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
        /// </summary>
        private IEnumerator UnloadPreviousScene()
        {
            yield return new WaitForSeconds(_fadeDuration);

            if (_currentlyLoadedScene != null) //would be null if the game was started in Initialization
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    //Unload the scene through its AssetReference, i.e. through the Addressable system
                    _currentlyLoadedScene.sceneReference.UnLoadScene();
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

            _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
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