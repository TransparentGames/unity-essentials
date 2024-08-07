
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TransparentGames.Essentials.Data;
using TransparentGames.Essentials.Shop;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
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

        public void AddItem(InventoryItem item, int index)
        {
            if (_items.TryGetValue(index, out var existingItem))
            {
                if (existingItem.ItemInstance.ItemInstanceId == item.ItemInstance.ItemInstanceId)
                {
                    existingItem.ItemInstance.RemainingUses += item.ItemInstance.RemainingUses;
                    Changed?.Invoke(item, true);
                    return;
                }

                _items[index] = item;
                Changed?.Invoke(item, true);
                return;
            }

            _items.Add(index, item);
            Changed?.Invoke(item, true);
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
                if (_items[key].ItemInstance.ItemInstanceId == item.ItemInstance.ItemInstanceId)
                {
                    _items.Remove(key);
                    Changed?.Invoke(item, false);
                    return;
                }
            }
        }
    }
}