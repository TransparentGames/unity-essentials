using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials
{
    public class ComponentBase : MonoBehaviour
    {
        public Entity owner;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ComponentBase), true)]
    public class ComponentBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var component = (ComponentBase)target;

            if (GUILayout.Button("Set Owner"))
            {
                var parent = FindParent(component.gameObject);
                var entity = parent.GetComponent<Entity>();
                if (parent != null)
                {
                    component.owner = entity;
                    EditorUtility.SetDirty(component);
                }
                else
                {
                    Debug.LogWarning("No parent found");
                    //component.owner = component.gameObject;
                }
            }
        }

        private void Reset()
        {
            var component = (ComponentBase)target;
            var parent = FindParent(component.gameObject);
            var entity = parent.GetComponent<Entity>();
            if (parent != null)
            {
                component.owner = entity;
                EditorUtility.SetDirty(component);
            }
            else
            {
                Debug.LogWarning("No parent found");
                //component.owner = component.gameObject;
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
