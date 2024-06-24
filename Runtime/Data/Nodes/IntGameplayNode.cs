using System;
using TransparentGames.Essentials;
using UnityEngine;

namespace TransparentGames.Data
{
    [CreateAssetMenu(fileName = "Gameplay Node", menuName = "Transparent Games/Data/Gameplay Node (int)", order = 0)]
    public class IntGameplayNode : BaseNode<int>
    {
        [SerializeField] private string key;
        [SerializeField] private int defaultValue;

        public override string DisplayValue => Value.ToString();
        public override int Value
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