using System;
using TMPro;
using TransparentGames.Essentials.Data.Nodes;
using UnityEngine;

namespace TransparentGames.Essentials.UI
{
    public class NodeLabel : MonoBehaviour
    {
        [SerializeField] private Node node;
        [SerializeField] private TextMeshProUGUI label;

        private void OnEnable()
        {
            Redraw();
            node.AddListener(Redraw);
        }

        private void OnDisable()
        {
            node.RemoveListener(Redraw);
        }

        private void Redraw()
        {
            label.text = node.DisplayValue;
        }
    }
}