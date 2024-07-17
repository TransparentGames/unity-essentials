
using System;
using System.Collections.Generic;
using TransparentGames.Essentials.Singletons;
using UnityEngine;
using TransparentGames.Essentials.Items;


#if !DISABLE_PLAYFABCLIENT_API && ENABLE_PLAYFABCLIENT_API
using PlayFab.ClientModels;
#else
using TransparentGames.Essentials.Dummy;
#endif

public class InventoryManager : PersistentMonoSingleton<InventoryManager>
{
    public event Action Changed;
    public IReadOnlyDictionary<string, InventoryItem> InventoryItems => _inventoryItems;
    /// <summary>
    /// Key: ItemInstanceId
    /// </summary>
    private Dictionary<string, InventoryItem> _inventoryItems = new();

    public void AddItem(ItemInstance itemInstance)
    {
        if (_inventoryItems.ContainsKey(itemInstance.ItemInstanceId))
        {
            Debug.Log($"Item [{itemInstance.ItemId}] already exists in inventory, updating...");
            UpdateItem(itemInstance.ItemInstanceId, itemInstance);
            return;
        }

        var inventoryItem = new InventoryItem(itemInstance);

        _inventoryItems.Add(itemInstance.ItemInstanceId, inventoryItem);
        Changed?.Invoke();
        Debug.Log($"Added item [{itemInstance.ItemId}] to inventory");
    }

    public bool TryGetItem(string itemId, out InventoryItem value)
    {
        foreach (var inventoryItem in _inventoryItems)
        {
            if (inventoryItem.Value.ItemInstance.ItemId == itemId)
            {
                value = inventoryItem.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

    public bool TryGetItemInstance(string itemInstanceId, out InventoryItem value)
    {
        if (_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
        {
            value = inventoryItem;
            return true;
        }

        value = null;
        return false;
    }

    public void UpdateItem(string itemInstanceId, ItemInstance itemInstance)
    {
        if (!_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
            return;

        inventoryItem.ItemInstance = itemInstance;
        Debug.Log($"Updated item [{itemInstanceId}] in inventory");

        Changed?.Invoke();
    }

    public void UpdateItem(string itemInstanceId, int remainingUses)
    {
        if (!_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
            return;

        inventoryItem.RemainingUses = remainingUses;
        Debug.Log($"Updated item [{itemInstanceId}] remaining uses to [{remainingUses}]");

        if (remainingUses <= 0)
        {
            _inventoryItems.Remove(itemInstanceId);
            Debug.Log($"Removed item [{itemInstanceId}] from inventory");
        }

        Changed?.Invoke();
    }

    public void RemoveItem(string itemInstanceId)
    {
        if (!_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
            return;

        _inventoryItems.Remove(itemInstanceId);
        Debug.Log($"Removed item [{itemInstanceId}] from inventory");

        Changed?.Invoke();
    }

    public void AddToItem(string itemInstanceId, int amount)
    {
        if (!_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
            return;

        UpdateItem(itemInstanceId, inventoryItem.RemainingUses + amount);
    }

    public void SubtractFromItem(string itemInstanceId, int amount)
    {
        if (!_inventoryItems.TryGetValue(itemInstanceId, out var inventoryItem))
            return;

        UpdateItem(itemInstanceId, inventoryItem.RemainingUses - amount);
    }
}