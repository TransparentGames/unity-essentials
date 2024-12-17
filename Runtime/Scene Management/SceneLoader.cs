using System;
using System.Collections;
using TMPro;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TransparentGames.Essentials.SceneManagement
{
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        public Action SceneReady;
        public bool IsLoading => _isLoading;

        [SerializeField] private GameSceneSO gameplayScene = default;
        [SerializeField] private GameObject loadingCanvasPrefab = default;

        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;
        private SceneInstance _gameplayManagerSceneInstance = new();
        private float _fadeDuration = 0.5f;
        private bool _isLoading = false;

        private GameObject _loadingCanvasInstance;
        private TextMeshProUGUI _loadingText;
        private Slider _loadingSlider;

#if UNITY_EDITOR
        public void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            _currentlyLoadedScene = currentlyOpenedLocation;

            if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
            {
                _gameplayManagerLoadingOpHandle = Addressables.LoadSceneAsync(gameplayScene.sceneReference, LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplaySceneCompleted;
            }
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
#endif

        protected override void Awake()
        {
            _loadingCanvasInstance = Instantiate(loadingCanvasPrefab);
            _loadingText = _loadingCanvasInstance.GetComponentInChildren<TextMeshProUGUI>();
            _loadingSlider = _loadingCanvasInstance.GetComponentInChildren<Slider>();
            _loadingCanvasInstance.SetActive(false);
            DontDestroyOnLoad(_loadingCanvasInstance);

            base.Awake();
        }

        public void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;
            ShowLoadingCanvas();

            if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                LoadGameplayManagers();
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        public void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (_isLoading)
                return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;
            ShowLoadingCanvas();

            if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
            {
                Addressables.Release(_gameplayManagerLoadingOpHandle);
            }

            StartCoroutine(UnloadPreviousScene());
        }

        private void LoadGameplayManagers()
        {
            _gameplayManagerLoadingOpHandle = Addressables.LoadSceneAsync(gameplayScene.sceneReference, LoadSceneMode.Additive, true);
            _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
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

        private IEnumerator UnloadPreviousScene()
        {
            yield return new WaitForSeconds(_fadeDuration);

            if (_currentlyLoadedScene != null)
            {
                if (_loadingOperationHandle.IsValid())
                {
                    Addressables.UnloadSceneAsync(_loadingOperationHandle);
                }
#if UNITY_EDITOR
                else
                {
                    SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            LoadNewScene();
        }

        private void LoadNewScene()
        {
            _loadingOperationHandle = Addressables.LoadSceneAsync(_sceneToLoad.sceneReference, LoadSceneMode.Additive, true);

            StartCoroutine(TrackLoadingProgress(_loadingOperationHandle));
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _currentlyLoadedScene = _sceneToLoad;

                Scene s = obj.Result.Scene;
                SceneManager.SetActiveScene(s);
                LightProbes.TetrahedralizeAsync();

                _isLoading = false;

                HideLoadingCanvas();

                StartGameplay();
            }
            else
            {
                Debug.LogError($"Failed to load new scene: {obj.OperationException}");
            }
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

        private void ShowLoadingCanvas()
        {
            if (_loadingCanvasInstance != null)
            {
                _loadingText.text = "Loading... 0%";
                _loadingSlider.value = 0;
                _loadingCanvasInstance.SetActive(true);
            }
        }

        private void HideLoadingCanvas()
        {
            if (_loadingCanvasInstance != null)
            {
                _loadingCanvasInstance.SetActive(false);
            }
        }

        private IEnumerator TrackLoadingProgress(AsyncOperationHandle<SceneInstance> handle)
        {
            float initialProgress = handle.PercentComplete;
            float normalizedProgress = 0f;

            while (!handle.IsDone)
            {
                // Normalize progress to start from 0%
                normalizedProgress = (handle.PercentComplete - initialProgress) / (1f - initialProgress);
                normalizedProgress = Mathf.Clamp01(normalizedProgress); // Ensure it's between 0 and 1
                _loadingText.text = $"Loading... {Mathf.CeilToInt(normalizedProgress * 100)}%";
                _loadingSlider.value = normalizedProgress;
                yield return null;
            }
            // Ensure the final progress is set to 99%
            _loadingText.text = "Loading... 99%";
            _loadingSlider.value = 0.99f;
            yield return null;

            OnNewSceneLoaded(handle);
        }
    }
}