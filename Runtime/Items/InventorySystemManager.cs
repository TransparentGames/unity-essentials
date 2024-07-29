using System.Collections.Generic;
using System.Linq;
using TransparentGames.Essentials.Dummy;
using TransparentGames.Essentials.Items;
using UnityEngine;

public class InventorySystemManager : MonoBehaviour
{
    [SerializeField] private ItemCollection itemCollection;

    private static Dictionary<string, ItemTemplate> _itemTemplates = new();

    private void Awake()
    {
        if (itemCollection == null)
        {
            Debug.LogError("ItemCollection is not set in InventorySystemManager");
            return;
        }

        foreach (var itemTemplate in itemCollection.ItemTemplates)
        {
            _itemTemplates.Add(itemTemplate.itemId, itemTemplate);
        }
    }

    public static void CreateItem(string itemId)
    {
        var itemTemplate = GetItemTemplate(itemId);
        var itemInstance = new ItemInstance
        {
            ItemId = itemId,
            RemainingUses = 1,
            ItemInstanceId = System.Guid.NewGuid().ToString(),
            ItemClass = itemTemplate.itemClass
        };

        InventoryManager.Instance.AddItem(itemInstance, itemTemplate);
    }

    public static ItemTemplate GetItemTemplate(string itemId)
    {
        if (_itemTemplates.ContainsKey(itemId))
        {
            return _itemTemplates[itemId];
        }

        Debug.LogError($"ItemTemplate with id [{itemId}] not found");
        return null;
    }

    public static List<InventoryItem> GetItemCategory(string category)
    {
        return InventoryManager.Instance.InventoryItems.Values.ToList().FindAll(item => item.ItemInstance.ItemClass == category);
    }
}