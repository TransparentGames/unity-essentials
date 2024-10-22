using System;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public class ItemInfo
    {
        // TODO: Add ItemUser id here so we could fetch here the proper item collection
        public ItemCollection ItemCollection
        {
            get => itemCollection;
            set
            {
                itemCollection = value;
                itemCollectionName = itemCollection.Name;
            }
        }
        private ItemCollection itemCollection;
        public string itemCollectionName;
        public int index;
    }
}