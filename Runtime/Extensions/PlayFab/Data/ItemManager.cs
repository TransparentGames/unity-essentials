#if !DISABLE_PLAYFABCLIENT_API && ENABLE_PLAYFABCLIENT_API
using System;
using System.Collections.Generic;
using UnityEngine;
using TransparentGames.Essentials.Items;
using PlayFab.ClientModels;
using PlayFab.ServerModels;


public static class ItemManager
{
    public static void ConsumeItem(string itemInstanceId, int count, Action successCallback, Action failureCallback = null)
    {
        InventoryManager.Instance.SubtractFromItem(itemInstanceId, count);
        ClientPlayFabHandler.ConsumeItem(itemInstanceId, count, (result) =>
        {
            successCallback?.Invoke();
        }, (error) =>
        {
            InventoryManager.Instance.AddToItem(itemInstanceId, count);
            failureCallback?.Invoke();
        });
    }

    public static void RemoveItems(List<string> itemInstanceIds, Action successCallback, Action failureCallback = null)
    {
        List<InventoryItem> inventoryItems = new();
        foreach (string itemInstanceId in itemInstanceIds)
        {
            if (!InventoryManager.Instance.TryGetItemInstance(itemInstanceId, out var inventoryItem))
                continue;
            inventoryItems.Add(inventoryItem);
            InventoryManager.Instance.RemoveItem(inventoryItem.ItemInstance.ItemInstanceId);
        }

        ServerPlayFabHandler.RevokeInventoryItems(ClientPlayFabHandler.PlayFabId, itemInstanceIds, (result) =>
        {
            successCallback?.Invoke();
        }, (error) =>
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                InventoryManager.Instance.AddItem(inventoryItem.ItemInstance);
            }
            failureCallback?.Invoke();
        });
    }

    public static void PurchaseItem(Offer offer, Action<List<string>> successCallback = null, Action failureCallback = null)
    {
        offer.currencyNode.Value -= offer.price;
        ClientPlayFabHandler.PurchaseItem(offer.rewardItem.item_id, offer.currencyNode.Key, offer.price, (result) =>
        {
            List<string> itemInstanceIds = new();
            foreach (ItemInstance item in result.Items)
            {
                Debug.Log($"Purchased item [{item.ItemId}] with ID [{item.ItemInstanceId}]");
                InventoryManager.Instance.AddItem(item);
                itemInstanceIds.Add(item.ItemInstanceId);
            }
            successCallback?.Invoke(itemInstanceIds);
        }, (error) =>
        {
            offer.currencyNode.Value += offer.price;
            failureCallback?.Invoke();
        });
    }

    public static void GrantItems(List<RewardItem> rewardItems, Action<List<string>> successCallback, Action failureCallback = null)
    {
        List<string> inventoryItemIds = new();
        rewardItems.ForEach((item) =>
        {
            if (item.data_type == DataType.Inventory)
                for (int i = 0; i < item.count; i++)
                {
                    inventoryItemIds.Add(item.item_id);
                }
            if (item.data_type == DataType.Currency)
                CurrencyManager.Instance.Add(item.item_id, item.count);
        });

        if (inventoryItemIds.Count == 0)
        {
            successCallback?.Invoke(new List<string>());
            return;
        }

        ServerPlayFabHandler.GrantItemsToUser(ClientPlayFabHandler.PlayFabId, inventoryItemIds, (result) =>
        {
            List<string> itemInstanceIds = new();
            List<ItemInstance> itemInstances = new();
            result.ItemGrantResults.ForEach((item) =>
            {
                itemInstances.Add(ConvertGrantedItemInstanceToItemInstance(item));
                itemInstanceIds.Add(item.ItemInstanceId);
            });

            DataManager.LoadInventory(itemInstances);
            successCallback?.Invoke(itemInstanceIds);
        }, (error) =>
        {
            failureCallback?.Invoke();
        });
    }

    private static ItemInstance ConvertGrantedItemInstanceToItemInstance(GrantedItemInstance grantedItem)
    {
        if (grantedItem == null)
        {
            return null;
        }

        return new ItemInstance
        {
            ItemInstanceId = grantedItem.ItemInstanceId,
            ItemId = grantedItem.ItemId,
            DisplayName = grantedItem.DisplayName,
            ItemClass = grantedItem.ItemClass,
            PurchaseDate = grantedItem.PurchaseDate,
            Expiration = grantedItem.Expiration,
            RemainingUses = grantedItem.RemainingUses,
            UsesIncrementedBy = grantedItem.UsesIncrementedBy,
            Annotation = grantedItem.Annotation,
            CatalogVersion = grantedItem.CatalogVersion,
            BundleParent = grantedItem.BundleParent,
            UnitCurrency = grantedItem.UnitCurrency,
            UnitPrice = grantedItem.UnitPrice,
            BundleContents = grantedItem.BundleContents,
            CustomData = grantedItem.CustomData,
        };
    }
}

#endif