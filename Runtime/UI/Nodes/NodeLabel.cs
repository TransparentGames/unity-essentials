using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TransparentGames.Data;
using UnityEngine;

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
