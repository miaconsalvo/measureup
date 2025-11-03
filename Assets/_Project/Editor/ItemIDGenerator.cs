using System.Collections.Generic;
using Mystie.Dressup;
using UnityEditor;
using UnityEngine;

namespace Mystie.MystEditor
{
    public class ItemIDGenerator : EditorWindow
    {
        [MenuItem("Tools/Mystie/Generate Item IDs")]
        public static void GenerateItemIDs()
        {
            GenerateIDs("Clothing");
        }

        public static void GenerateIDs(string path)
        {
            ItemScriptable[] allItems = Resources.LoadAll<ItemScriptable>(path);

            if (allItems.Length == 0)
            {
                EditorUtility.DisplayDialog("No Clothing Found",
                    $"No {typeof(ItemScriptable).Name} found in Resources/{path}/", "OK");
                return;
            }

            HashSet<string> usedIds = new HashSet<string>();
            int newIdCount = 0;
            List<string> duplicateItems = new List<string>();

            Debug.Log($"=== Clothing ID Generation Started ===");
            Debug.Log($"Scanning {allItems.Length} clothing items...\n");

            foreach (ItemScriptable item in allItems)
            {
                if (item == null) continue;

                // Check if ID exists and is unique
                if (!string.IsNullOrEmpty(item.id) && !usedIds.Contains(item.id))
                {
                    usedIds.Add(item.id);
                }
                else
                {
                    // Generate new ID if missing or duplicate
                    string newId = GenerateUniqueID(usedIds);

                    if (!string.IsNullOrEmpty(item.id))
                    {
                        duplicateItems.Add($"  • {item.name} (old ID: {item.id})");
                    }

                    item.id = newId;
                    usedIds.Add(newId);
                    newIdCount++;

                    EditorUtility.SetDirty(item);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Print results
            Debug.Log($"=== Clothing ID Generation Complete ===");
            Debug.Log($"Total clothing scanned: {allItems.Length}");
            Debug.Log($"New IDs created: {newIdCount}");
            Debug.Log($"Existing unique IDs preserved: {allItems.Length - newIdCount}");

            if (duplicateItems.Count > 0)
            {
                Debug.Log($"\nClothing with duplicate/invalid IDs that were regenerated ({duplicateItems.Count}):");
                foreach (var item in duplicateItems)
                    Debug.Log(item);
            }

            EditorUtility.DisplayDialog("Success",
                $"ID generation complete!\n\n" +
                $"Scanned: {allItems.Length}\n" +
                $"New IDs: {newIdCount}\n" +
                $"Duplicates fixed: {duplicateItems.Count}",
                "OK");
        }

        private static string GenerateUniqueID(HashSet<string> usedIds)
        {
            string newId;
            do
            {
                newId = System.Guid.NewGuid().ToString();
            } while (usedIds.Contains(newId));

            return newId;
        }
    }
}
