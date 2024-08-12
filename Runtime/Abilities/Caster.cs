
using System;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace TransparentGames.Essentials.Abilities
{
    public abstract class Caster : MonoBehaviour, IComponent
    {
        public virtual event Action Ready;
        public GameObject Owner { get; set; }

        public abstract Animator Animator { get; }
        public abstract bool IsBusy { get; }
        public abstract bool CanCancel { get; }
        public abstract bool CanHardCancel { get; }

        public abstract void Equip(AbilityTemplate abilityTemplate);
        public abstract void Unequip();
        public abstract void Cast(GameObject target = null);
        public abstract bool CanCast();
        public abstract void Cancel();

        protected void OnReady()
        {
            Ready?.Invoke();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Caster), true)]
    public class CasterEditor : Editor
    {
        private AbilityTemplate _abilityTemplateHolder;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);

            if (target is not Caster caster)
                return;

            EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            _abilityTemplateHolder = EditorGUILayout.ObjectField("Ability Template", _abilityTemplateHolder, typeof(AbilityTemplate), false) as AbilityTemplate;
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Equip Ability"))
            {
                caster.Equip(_abilityTemplateHolder);
            }

            if (GUILayout.Button("Unequip Ability"))
            {
                caster.Unequip();
            }
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Cast"))
                caster.Cast();
        }
    }
#endif
}