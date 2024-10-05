
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
        [SerializeField] private List<Price> startingItems;

        private void Awake()
        {
            foreach (var price in startingItems)
            {
                AddItem(InventorySystemManager.CreateItem(price.itemTemplate.itemId, price.amount));
            }
        }

        public ItemCollection GetItemCollection(string category = "main")
        {
            foreach (var itemCollection in itemCollections)
            {
                if (itemCollection.Category == category)
                {
                    return itemCollection;
                }
            }

            return null;
        }

        public bool AddItem(InventoryItem item)
        {
            foreach (var itemCollection in itemCollections)
            {
                if (itemCollection.Category == item.ItemTemplate.itemClass)
                {
                    return itemCollection.TryAddItem(item);
                }
            }

            return GetItemCollection().TryAddItem(item);
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

        public void RemoveItem(InventoryItem item)
        {
            foreach (var itemCollection in itemCollections)
            {
                if (itemCollection.Category == item.ItemTemplate.itemClass)
                {
                    itemCollection.RemoveItem(item);
                    return;
                }
            }

            GetItemCollection().RemoveItem(item);
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