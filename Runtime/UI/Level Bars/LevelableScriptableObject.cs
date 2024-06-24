using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levelable Scriptable Object", menuName = "Transparent Games/Configs/Levelable", order = 0)]
public class LevelableScriptableObject : ScriptableObject
{
    public List<int> levels;
}