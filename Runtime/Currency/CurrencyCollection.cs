using System.Collections.Generic;
using TransparentGames.Essentials.Data.Nodes;
using TransparentGames.Essentials.Singletons;
using UnityEngine;

namespace TransparentGames.Essentials.Currency
{
    [CreateAssetMenu(fileName = "New Currency Collection", menuName = "Transparent Games/Data/Currency Collection", order = 0)]
    public class CurrencyCollection : ScriptableObjectSingleton<CurrencyCollection>
    {
        public List<CurrencyNode> CurrencyNodes => currencyNodes;

        [SerializeField] private List<CurrencyNode> currencyNodes;

        public CurrencyNode GetCurrencyNode(string key)
        {
            return currencyNodes.Find(currencyNode => currencyNode.Key == key);
        }
    }
}