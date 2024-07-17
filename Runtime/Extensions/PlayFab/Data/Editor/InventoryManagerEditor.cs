using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var inventoryManager = (InventoryManager)target;

        EditorGUI.BeginDisabledGroup(true);

        foreach (var entry in inventoryManager.InventoryItems)
        {
            EditorGUILayout.IntField(entry.Value.ItemTemplate.itemId, entry.Value.RemainingUses);
        }
        EditorGUI.EndDisabledGroup();
    }
}