using System.Collections.Generic;
using TransparentGames.Essentials;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials
{
    public class Entity : MonoBehaviour, IComponentParent
    {
        //public List<ComponentBase> Components = new();
    }

    // #if UNITY_EDITOR
    //     [CustomEditor(typeof(Entity), true)]
    //     public class EntityEditor : Editor
    //     {

    //         private Entity _entity;
    //         public override void OnInspectorGUI()
    //         {
    //             base.OnInspectorGUI();

    //             _entity = (Entity)target;

    //             GUILayout.Space(10);
    //             GUILayout.Label("Editor", EditorStyles.boldLabel);

    //             if (GUILayout.Button("Get All Components"))
    //             {
    //                 _entity.Components = _entity.GetComponentsInChildren<ComponentBase>().ToList();
    //                 foreach (var component in _entity.Components)
    //                 {
    //                     component.owner = _entity.gameObject;
    //                 }

    //                 EditorUtility.SetDirty(_entity);
    //             }
    //         }
    //     }
    // #endif
}