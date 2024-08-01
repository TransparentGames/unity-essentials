
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    public class Inventory : MonoBehaviour
    {
        public List<ItemCollection> ItemCollections => itemCollections;

        [SerializeField] private string inventoryId;
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
                    return itemCollection.AddItem(item);
                }
            }

            return GetItemCollection().AddItem(item);
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
    }

    [Serializable]
    public class ItemCollection
    {
        public int SlotAmount => slotAmount;
        public string Category => category;
        public event Action<InventoryItem, bool> Changed;
        public IReadOnlyDictionary<int, InventoryItem> Items => _items;

        [SerializeField] private int slotAmount;
        [SerializeField] private string category;

        private readonly Dictionary<int, InventoryItem> _items = new();

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count >= slotAmount && item.ItemTemplate.IsUnique)
            {
                Debug.LogWarning("Inventory is full");
                return false;
            }

            for (int i = 0; i < slotAmount; i++)
            {
                if (_items.ContainsKey(i))
                {
                    if (_items[i].ItemTemplate.IsUnique)
                        continue;

                    if (_items[i].ItemInstance.ItemId != item.ItemInstance.ItemId)
                        continue;

                    _items[i].ItemInstance.RemainingUses += item.ItemInstance.RemainingUses;
                    Changed?.Invoke(item, true);
                    return true;
                }

                _items.Add(i, item);
                Changed?.Invoke(item, true);
                return true;
            }

            return false;
        }

        public InventoryItem GetItem(int index)
        {
            if (!_items.ContainsKey(index))
            {
                return null;
            }

            return _items[index];
        }

        public void RemoveItem(InventoryItem item)
        {
            foreach (var key in _items.Keys)
            {
                if (_items[key] == item)
                {
                    _items.Remove(key);
                    Changed?.Invoke(item, false);
                    return;
                }
            }
        }
    }
}