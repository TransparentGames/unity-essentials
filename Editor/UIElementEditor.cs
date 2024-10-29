
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CustomEditor(typeof(UIElement), true), CanEditMultipleObjects]
    public class UIElementEditor : Editor
    {
        private UIElement _view;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _view = target as UIElement;

            if (_view == null)
                return;

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            if (_view.State == null)
            {
                EditorGUILayout.HelpBox("No UI State assigned to this UIElement.", MessageType.Warning);
                if (GUILayout.Button("Create UI State"))
                {
                    CreateUiState();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("UI State assigned to this UIElement.", MessageType.None);
            }
        }

        private void CreateUiState()
        {
            string prefabPath = string.Empty;

            // Check if the prefab is currently being edited in Prefab Mode
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.prefabContentsRoot == _view.gameObject)
            {
                // Get the path of the prefab being edited in Prefab Mode
                prefabPath = prefabStage.assetPath;
            }
            // Check if the object is part of a prefab instance in the scene
            else if (PrefabUtility.IsPartOfPrefabInstance(_view))
            {
                prefabPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabInstanceHandle(_view));
            }
            // Check if the object is part of a prefab asset
            else if (PrefabUtility.IsPartOfPrefabAsset(_view))
            {
                prefabPath = AssetDatabase.GetAssetPath(_view);
            }

            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError("This object is not part of a prefab!");
                return;
            }

            Debug.Log("Prefab Path: " + prefabPath);

            // Get the directory of the prefab
            string folderPath = Path.GetDirectoryName(prefabPath);

            // Create a ScriptableObject in that folder
            UIState asset = CreateInstance<UIState>();

            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{_view.name} UI State.asset");

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            // Use SerializedObject to modify the private serialized field
            SerializedObject serializedView = new SerializedObject(_view);
            SerializedProperty stateProperty = serializedView.FindProperty("state");

            if (stateProperty != null)
            {
                stateProperty.objectReferenceValue = asset;
                serializedView.ApplyModifiedProperties(); // Apply the assignment
            }
            else
            {
                Debug.LogError("Failed to find the 'state' property on the UIElement component.");
            }

            // Mark the prefab as dirty so that changes are saved
            EditorUtility.SetDirty(_view);

            // Save the modified prefab if it's in Prefab Mode
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }

            Debug.Log($"ScriptableObject created and assigned at: {assetPath}");
        }

    }
}