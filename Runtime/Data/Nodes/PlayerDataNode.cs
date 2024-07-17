using System;
using UnityEngine;

namespace TransparentGames.Essentials.Data.Nodes
{
    [CreateAssetMenu(fileName = "Player Data Node", menuName = "Transparent Games/Data/Player Data Node", order = 0)]
    public class PlayerDataNode : BaseNode<string>
    {
        public string Key => key;

        [SerializeField] private string key;
        [SerializeField, TextArea] private string defaultValue;


        public override string DisplayValue => Value.ToString();
        public override string Value
        {
            get
            {
                if (PlayerDataManager.InstanceExists)
                {
                    return PlayerDataManager.Instance.GetPlayerData(key, defaultValue).Value;
                }

                return defaultValue;
            }
            set
            {
                if (PlayerDataManager.InstanceExists)
                {
                    PlayerDataManager.Instance.GetPlayerData(key, defaultValue).Value = value;
                }
            }
        }

        public override void AddListener(Action callback)
        {
            PlayerDataManager.Initialized(() => PlayerDataManager.Instance.GetPlayerData(key, defaultValue).Changed += callback);
        }

        public override void RemoveListener(Action callback)
        {
            if (PlayerDataManager.InstanceExists)
                PlayerDataManager.Instance.GetPlayerData(key, defaultValue).Changed -= callback;
        }
    }
}