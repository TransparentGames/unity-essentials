using UnityEngine;
using UnityEditor;

namespace TransparentGames.Essentials.Data
{
    [CustomEditor(typeof(CachedDataManager))]
    public class CachedDataManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var dataSaveManager = (CachedDataManager)target;

            if (dataSaveManager?.Properties.Count == 0)
            {
                EditorGUILayout.HelpBox("No properties found", MessageType.Info);
                return;
            }

            EditorGUI.BeginDisabledGroup(true);

            foreach (var entry in dataSaveManager.Properties)
            {
                if (entry.Value is IDataProperty<int> intProperty)
                {
                    EditorGUILayout.IntField(entry.Key, intProperty.Value);
                }
                else if (entry.Value is IDataProperty<bool> boolProperty)
                {
                    EditorGUILayout.Toggle(entry.Key, boolProperty.Value);
                }
                else if (entry.Value is IDataProperty<float> floatProperty)
                {
                    EditorGUILayout.FloatField(entry.Key, floatProperty.Value);
                }
                else if (entry.Value is IDataProperty<string> stringProperty)
                {
                    EditorGUILayout.TextField(entry.Key, stringProperty.Value);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}