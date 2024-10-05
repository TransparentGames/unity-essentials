using System;

namespace TransparentGames.Essentials.Items
{
    [Serializable]
    public struct ItemInfo
    {
        public ItemUser itemUser;
        public ItemCollection itemCollection;
        public int index;
    }
}