
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TransparentGames.Essentials.Items
{
    public class Inventory : MonoBehaviour
    {
        public List<ItemCollection> ItemCollections => itemCollections;

        [SerializeField] private ItemUser itemUser;
        [SerializeField] private List<ItemCollection> itemCollections;

        public ItemCollection GetItemCollection(string collectionName)
        {
            foreach (var itemCollection in itemCollections)
            {
                if (itemCollection.Name == collectionName)
                {
                    return itemCollection;
                }
            }

            return null;
        }

        public ItemCollection GetDefaultItemCollection(InventoryItem inventoryItem)
        {
            foreach (var itemCollection in itemCollections)
            {
                if (itemCollection.CanAddItem(inventoryItem))
                {
                    return itemCollection;
                }
            }

            return null;
        }

        public bool AddItem(InventoryItem item)
        {
            return GetDefaultItemCollection(item).TryAddItem(item);
        }

        public InventoryItem GetItem(string itemId)
        {
            foreach (var itemCollection in itemCollections)
            {
                foreach (var item in itemCollection.Items)
                {
                    if (item.Value.ItemInstance.ItemId == itemId)
                    {
                        return item.Value;
                    }
                }
            }

            return null;
        }

#if UNITY_EDITOR

        public void OnValidate()
        {
            foreach (var itemCollection in itemCollections)
            {
                itemCollection?.EditorSetItemUser(itemUser);
            }

            EditorUtility.SetDirty(this); // Mark as dirty to save
        }
#endif
    }
}