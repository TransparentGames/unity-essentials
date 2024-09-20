using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "New Item Collection", menuName = "Transparent Games/Items/Item Collection", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemTemplate> ItemTemplates
        {
            get
            {
                var templates = new List<ItemTemplate>();
                templates.AddRange(itemTemplates);
                foreach (var externalDatabase in externalDatabases)
                {
                    templates.AddRange(externalDatabase.ItemTemplates);
                }

                return templates;
            }
        }

        [SerializeField] private List<ItemTemplate> itemTemplates;
        [SerializeField] private List<ItemDatabase> externalDatabases;

        public ItemTemplate GetItemTemplate(string itemId)
        {
            foreach (var externalDatabase in externalDatabases)
            {
                var itemTemplate = externalDatabase.GetItemTemplate(itemId);
                if (itemTemplate != null)
                    return itemTemplate;
            }

            return itemTemplates.Find(itemTemplate => itemTemplate.itemId == itemId);
        }
    }
}