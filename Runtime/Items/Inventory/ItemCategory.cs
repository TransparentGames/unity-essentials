using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CreateAssetMenu(fileName = "Item Category", menuName = "Transparent Games/Items/New Item Category", order = 0)]
    public class ItemCategory : ScriptableObject
    {
        public string Name => categoryName;

        [SerializeField] private string categoryName;
    }
}