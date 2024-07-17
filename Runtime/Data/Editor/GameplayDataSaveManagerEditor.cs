using UnityEngine;
using UnityEditor;

namespace TransparentGames.Essentials.Data
{
    [CustomEditor(typeof(GameplayDataSaveManager))]
    public class GameplayDataSaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameplayDataSaveManager = (GameplayDataSaveManager)target;

            EditorGUI.BeginDisabledGroup(true);

            foreach (var entry in gameplayDataSaveManager.Properties)
            {
                if (entry.Value is IDataProperty<int> intProperty)
                {
                    EditorGUILayout.IntField(entry.Key, intProperty.Value);
                }
                else if (entry.Value is IDataProperty<bool> boolProperty)
                {
                    EditorGUILayout.Toggle(entry.Key, boolProperty.Value);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}