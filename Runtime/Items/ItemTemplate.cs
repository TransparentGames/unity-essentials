using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "New Item Template", menuName = "Transparent Games/Items/Item Template", order = 0)]
    public class ItemTemplate : PlayfabItemTemplate
    {
        public virtual string Name => itemName;
        public virtual string Description => description;
        [Space]
        public Sprite icon;
        [SerializeField] protected string itemName;
        [SerializeField, TextArea] protected string description;
        public string itemClass;
        public bool IsUnique;
    }
}