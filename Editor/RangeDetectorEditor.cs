
using UnityEditor;
using UnityEngine;

namespace TransparentGames.Essentials.Detection
{
    [CustomEditor(typeof(RangeDetector), true)]
    public class RangeDetectorEditor : ComponentBaseEditor
    {
        private bool _showDetectedList;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            var detector = (IDetector)target;


            var detected = detector.AllDetected;

            if (detected.Count <= 0)
            {
                GUILayout.Label($"Nothing detected");
            }
            else
            {
                _showDetectedList =
                    EditorGUILayout.Foldout(_showDetectedList, $"Detected ({detected.Count}): ");
                if (_showDetectedList)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    for (int i = 0; i < detected.Count; i++)
                    {
                        EditorGUILayout.ObjectField(detected[i].Owner, typeof(Transform), true);
                    }

                    EditorGUI.EndDisabledGroup();
                }
            }
        }
    }
}