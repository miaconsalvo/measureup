using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using Mystie.Dressup;
using UnityEngine;

namespace Mystie
{
    public static class SaveDataManager
    {
        public static GameData gameData { get; private set; }

        public static void SaveGameData()
        {
            if (gameData == null)
            {
                Debug.Log("SaveDataManager: No game data to be saved.");
                return;
            }

            SaveManager.SaveGameToFile(gameData);
        }

        public static void LoadGameData(GameData newGameData)
        {
            if (newGameData == null)
            {
                Debug.Log("SaveDataManager: No game data to be loaded.");
                return;
            }

            gameData = newGameData;

            GameManager.playerName = gameData.playerName;
            EpisodeManager.Instance.SetEpisodeIndex(gameData.episodeIndex);
        }

        #region Load Data



        #endregion

        #region Save Data

        public static void SavePlayerName(string newPlayerName)
        {
            gameData.playerName = newPlayerName;
        }

        public static void SaveEpisodeIndex(int newIndex)
        {
            gameData.episodeIndex = newIndex;
        }

        public static void SaveMoneyAmount(int newMoneyAmount)
        {
            gameData.moneyAmount = newMoneyAmount;
        }

        public static void SaveReaction(string name, Reaction reaction)
        {
            gameData.reactions[name] = reaction;
        }

        public static void SaveInventory(List<ItemScriptable> ownedClothing, List<ClothingKitScriptable> ownedKits)
        {
            gameData.inventoryData.ownedClothingIds = ownedClothing.Select(c => c.id).ToList();
            gameData.inventoryData.ownedKitIds = ownedKits.Select(k => k.id).ToList();
        }

        #endregion
    }

    [System.Serializable]
    public class GameData
    {
        public static string DEFAULT_NAME = "Cindy";
        public static int STARTING_MONEY = 1000;

        public string playerName;
        public int moneyAmount;
        public int episodeIndex;
        public string versionNumber;
        public InventoryData inventoryData;
        public Dictionary<string, Reaction> reactions;

        public GameData()
        {
            playerName = DEFAULT_NAME;
            moneyAmount = STARTING_MONEY;
            episodeIndex = 0;
            versionNumber = Application.version;
            inventoryData = new InventoryData();
            reactions = new Dictionary<string, Reaction>();
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<string> ownedClothingIds = new List<string>();
        public List<string> ownedKitIds = new List<string>();


    }

}
