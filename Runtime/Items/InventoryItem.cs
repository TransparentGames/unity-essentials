using System;
using TransparentGames.Essentials.Items;

#if !DISABLE_PLAYFABCLIENT_API && ENABLE_PLAYFABCLIENT_API
using PlayFab.ClientModels;
#else
using TransparentGames.Essentials.Dummy;
#endif

namespace TransparentGames.Essentials.Items
{
    public class InventoryItem
    {
        public Action Changed;
        private ItemTemplate _itemTemplate;
        private ItemInstance _itemInstance;
        private ItemInfo _itemInfo;

        public InventoryItem()
        {
            _itemInstance = new()
            {
                RemainingUses = 0
            };
        }

        public InventoryItem(ItemInstance itemInstance, ItemTemplate itemTemplate)
        {
            _itemTemplate = itemTemplate;
            _itemInstance = itemInstance;
        }

        public ItemInstance ItemInstance
        {
            get => _itemInstance;
            set
            {
                _itemInstance = value;
                Changed?.Invoke();
            }
        }

        public ItemTemplate ItemTemplate => _itemTemplate;

        public ItemInfo ItemInfo
        {
            get => _itemInfo;
            set
            {
                _itemInfo = value;
                Changed?.Invoke();
            }
        }

        public int RemainingUses
        {
            get => _itemInstance.RemainingUses ?? 0;
            set
            {
                _itemInstance.RemainingUses = value;
                Changed?.Invoke();
            }
        }
    }
}

