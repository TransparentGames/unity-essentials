using System.Collections.Generic;
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Currency
{
    [CreateAssetMenu(fileName = "New Currency Collection", menuName = "Transparent Games/Data/Currency Collection", order = 0)]
    public class CurrencyCollection : ScriptableObject
    {
        public List<CurrencyNode> CurrencyNodes => currencyNodes;

        [SerializeField] private List<CurrencyNode> currencyNodes;

        public CurrencyNode GetCurrencyNode(string itemId)
        {
            return currencyNodes.Find(currencyNode => currencyNode.Key == itemId);
        }
    }
}