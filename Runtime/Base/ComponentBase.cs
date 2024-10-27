using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials
{
    public class ComponentBase : MonoBehaviour
    {
        [HideInInspector] public Entity owner;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ComponentBase), true)]
    public class ComponentBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            var component = (ComponentBase)target;
            if (component.owner == null)
            {
                EditorGUILayout.HelpBox("Owner is not set", MessageType.Warning);

                if (GUILayout.Button("Set Owner"))
                {
                    SetOwner(component);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Owner is set", MessageType.Info);
            }
        }

        private void SetOwner(ComponentBase component)
        {
            var parent = FindParent(component.gameObject);
            if (parent != null)
            {
                var entity = parent.GetComponent<Entity>();
                if (entity != null)
                {
                    component.owner = entity;
                    EditorUtility.SetDirty(component);
                }
                else
                {
                    Debug.LogError("No Entity component found on parent.");
                }
            }
            else
            {
                Debug.LogError("No parent found.");
            }
        }

        private GameObject FindParent(GameObject go)
        {
            if (go.GetComponent<IComponentParent>() != null)
            {
                return go;
            }

            if (go.transform.parent == null)
            {
                return null;
            }

            return FindParent(go.transform.parent.gameObject);
        }
    }
#endif
}