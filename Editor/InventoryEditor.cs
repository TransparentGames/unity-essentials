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
                GUILayout.Label(item.Category, EditorStyles.boldLabel);
                foreach (var inventoryItem in item.Items.Values)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(inventoryItem.ItemTemplate, typeof(ItemTemplate), false);
                    EditorGUILayout.IntField(inventoryItem.ItemInstance.ItemInstanceId, inventoryItem.RemainingUses);
                    GUILayout.EndHorizontal();
                }

            }
        }
    }
}