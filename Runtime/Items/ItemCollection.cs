using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "New Item Collection", menuName = "Transparent Games/Items/Item Collection", order = 0)]
    public class ItemCollection : ScriptableObject
    {
        public List<ItemTemplate> ItemTemplates => itemTemplates;

        [SerializeField] private List<ItemTemplate> itemTemplates;

        public ItemTemplate GetItemTemplate(string itemId)
        {
            return itemTemplates.Find(itemTemplate => itemTemplate.itemId == itemId);
        }
    }
}