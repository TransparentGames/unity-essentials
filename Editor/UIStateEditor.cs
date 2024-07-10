
using UnityEditor;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    [CustomEditor(typeof(UIState))]
    public class UIStateEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not UIState uiState)
                return;

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            if (GUILayout.Button("Open"))
                uiState.Open();

            if (GUILayout.Button("Close"))
                uiState.Close();
        }
    }
}