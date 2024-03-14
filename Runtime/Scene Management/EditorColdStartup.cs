using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TransparentGames.Core
{
    /// <summary>
    /// Allows a "cold start" in the editor, when pressing Play and not passing from the Initialization scene.
    /// </summary> 
    public class EditorColdStartup : MonoBehaviour
    {
#if UNITY_EDITOR && !UNITY_SERVER
        [SerializeField] private GameSceneSO thisSceneSO = default;
        [SerializeField] private GameSceneSO persistentManagersSceneSO = default;

        private bool isColdStart = false;
        private void Awake()
        {
            if (!SceneManager.GetSceneByName(persistentManagersSceneSO.sceneReference.editorAsset.name).isLoaded)
            {
                isColdStart = true;
            }
        }

        private void Start()
        {
            if (isColdStart)
            {
                persistentManagersSceneSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
            }
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            SceneLoader.Instance.LocationColdStartup(thisSceneSO);
        }
#endif
    }
}