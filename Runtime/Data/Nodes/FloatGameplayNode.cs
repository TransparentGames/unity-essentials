using System;
using TransparentGames.Essentials;
using UnityEngine;

namespace TransparentGames.Essentials.Data.Nodes
{
    [CreateAssetMenu(fileName = "Gameplay Node", menuName = "Transparent Games/Data/Gameplay Node (float)", order = 0)]
    public class FloatGameplayNode : BaseNode<float>
    {
        [SerializeField] private string key;
        [SerializeField] private float defaultValue = 0f;
        [SerializeField] private float displayMultiplier = 1f;
        [SerializeField] private string displayFormat = "F2";
        [SerializeField] private string suffix = "";

        public override string DisplayValue => (Value * displayMultiplier).ToString(displayFormat) + suffix;
        public override float Value
        {
            get
            {
                if (GameplayDataSaveManager.InstanceExists)
                {
                    return GameplayDataSaveManager.Instance.GetProperty(key, defaultValue).Value;
                }

                return defaultValue;
            }
            set
            {
                if (GameplayDataSaveManager.InstanceExists)
                {
                    GameplayDataSaveManager.Instance.GetProperty(key, defaultValue).Value = value;
                }
            }
        }

        public override void AddListener(Action callback)
        {
            GameplayDataSaveManager.Initialized(() => GameplayDataSaveManager.Instance.GetProperty(key, defaultValue).Changed += callback);
        }

        public override void RemoveListener(Action callback)
        {
            if (GameplayDataSaveManager.InstanceExists)
                GameplayDataSaveManager.Instance.GetProperty(key, defaultValue).Changed -= callback;
        }
    }
}