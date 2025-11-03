using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mystie.Dressup
{
    public class InventoryManager : MonoBehaviour
    {
        public event Action<int> onMoneyUpdated;
        public static Dictionary<string, ItemScriptable> clothingLookup;
        public static Dictionary<string, ClothingKitScriptable> kitLookup;

        [field: SerializeField] public int moneyAmount { get; private set; }
        [field: SerializeField] public List<ItemScriptable> ownedClothing { get; private set; }
        [field: SerializeField] public List<ClothingKitScriptable> ownedKits { get; private set; }

        public static void LoadClothingData()
        {
            // Load all ScriptableObjects from Resources folder
            ItemScriptable[] allClothing = Resources.LoadAll<ItemScriptable>("Clothing");
            clothingLookup = allClothing.ToDictionary(c => c.id);

            ClothingKitScriptable[] allKits = Resources.LoadAll<ClothingKitScriptable>("Clothing Kits");
            kitLookup = allKits.ToDictionary(k => k.id);
        }

        public void Initialize(EpisodeScriptable episode)
        {
            LoadInventory(SaveDataManager.gameData);
            AddClothes(episode.startingClothes);
        }

        public void AddClothes(List<ItemScriptable> newClothes)
        {
            ownedClothing.AddRange(newClothes);
        }

        public void AddClothingKit(ClothingKitScriptable newClothingKit)
        {
            Debug.Log("Added kit " + newClothingKit.ToString());
            //foreach (ItemScriptable garment in newClothingKit.garments)
            //Debug.Log(garment.ToString());
            ownedKits.Add(newClothingKit);
            AddClothes(newClothingKit.garments);
        }

        public void GainMoney(int amount)
        {
            moneyAmount += amount;
            moneyAmount = Mathf.Max(moneyAmount, 0);

            onMoneyUpdated?.Invoke(moneyAmount);
            Debug.Log($"{amount}$ {(amount >= 0 ? "gained" : "spent")}. New total is {moneyAmount}$.");
        }

        public void LoadInventory(GameData data)
        {
            if (data == null) return;

            moneyAmount = data.moneyAmount;
            ownedClothing = data.inventoryData.ownedClothingIds
                .Where(id => clothingLookup.ContainsKey(id))
                .Select(id => clothingLookup[id])
                .ToList();

            ownedKits = data.inventoryData.ownedKitIds
                .Where(id => kitLookup.ContainsKey(id))
                .Select(id => kitLookup[id])
                .ToList();
        }

        public void SaveInventory()
        {
            SaveDataManager.SaveMoneyAmount(moneyAmount);
            SaveDataManager.SaveInventory(ownedClothing, ownedKits);
        }
    }
}
