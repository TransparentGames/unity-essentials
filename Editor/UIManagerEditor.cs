
using UnityEditor;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            var uiManager = (UIManager)target;

            foreach (var uiState in uiManager.History)
            {
                EditorGUILayout.ObjectField(uiState, typeof(UIState), true);
            }
        }
    }
}