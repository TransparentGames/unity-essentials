using UnityEditor;
using UnityEngine;

namespace TransparentGames.Essentials.Items
{
    [CustomEditor(typeof(Inventory), true)]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUILayout.Label("Editor", EditorStyles.boldLabel);

            if (target is not Inventory inventory)
                return;

            foreach (var item in inventory.ItemCollections)
            {
                GUILayout.Label(item.Name, EditorStyles.boldLabel);
                foreach (var inventoryItem in item.Items.Values)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(inventoryItem.ItemTemplate, typeof(ItemTemplate), false);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.IntField(inventoryItem.ItemInstance.ItemInstanceId, inventoryItem.RemainingUses);
                    if (GUILayout.Button("Remove"))
                    {
                        item.RemoveItem(inventoryItem);
                    }
                    GUILayout.EndHorizontal();
                }

            }
        }
    }
}