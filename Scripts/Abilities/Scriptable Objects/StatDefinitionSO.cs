
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Abilities.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Stat Definition", menuName = "Abilities/Stat Definition", order = 1)]
    public class StatDefinitionSO : ScriptableObject
    {
        [SerializeField] private string statName;
        [SerializeField] private Image icon;
        [SerializeField] private string description;
    }
}