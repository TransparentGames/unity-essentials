using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "New Item Template", menuName = "Transparent Games/Items/Item Template", order = 0)]
    public class ItemTemplate : PlayfabItemTemplate
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        public virtual string Name => itemName;
        public virtual string Description => description;
        [Space]
        /// <summary>
        /// The icon of the item.
        /// </summary>
        public Sprite icon;
        [Tooltip("Used for categorizing items")] public string itemClass;
        /// <summary>
        /// The maximum amount of this item that can be stacked in a single inventory slot.
        /// </summary>
        public bool IsUnique;
        [SerializeField] protected string itemName;
        [SerializeField, TextArea] protected string description;
    }
}