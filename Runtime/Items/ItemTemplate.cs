using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "New Item Template", menuName = "Transparent Games/Items/Item Template", order = 0)]
    public class ItemTemplate : PlayfabItemTemplate
    {
        [Space]
        public Sprite icon;
        public string itemName;
    }
}