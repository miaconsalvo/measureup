using System;
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
            gameData.EnsureInitialized();

            if (!string.IsNullOrEmpty(gameData.playerName))
                GameManager.playerName = gameData.playerName;

            EpisodeManager episodeManager = EpisodeManager.Instance;
            if (episodeManager != null) episodeManager.SetEpisodeIndex(gameData.episodeIndex);
        }

        #region Save Data

        public static void SavePlayerName(string newPlayerName)
        {
            gameData.playerName = newPlayerName;
            GameManager.playerName = newPlayerName;
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
            if (string.IsNullOrEmpty(name)) return;

            ReactionEntry entry = gameData.reactions.FirstOrDefault(
                e => string.Equals(e.name, name, StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                entry = new ReactionEntry { name = name };
                gameData.reactions.Add(entry);
            }

            entry.reaction = (int)reaction;
        }

        public static Reaction GetReaction(string name)
        {
            if (gameData == null || string.IsNullOrEmpty(name)) return Reaction.Neutral;

            ReactionEntry entry = gameData.reactions.FirstOrDefault(
                e => string.Equals(e.name, name, StringComparison.OrdinalIgnoreCase));

            return entry != null ? (Reaction)entry.reaction : Reaction.Neutral;
        }

        public static void SaveInventory(List<ItemScriptable> ownedClothing, List<ClothingKitScriptable> ownedKits)
        {
            gameData.inventoryData.ownedClothingIds = ownedClothing.Select(c => c.id).ToList();
            gameData.inventoryData.ownedKitIds = ownedKits.Select(k => k.id).ToList();
        }

        public static void SaveTags(string name, List<ClothingTag> tags)
        {
            if (string.IsNullOrEmpty(name)) return;

            TagEntry entry = gameData.tags.FirstOrDefault(
                e => string.Equals(e.contestantId, name, StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                entry = new TagEntry { contestantId = name };
                gameData.tags.Add(entry);
            }

            entry.tagNames = tags == null
                ? new List<string>()
                : tags.Where(t => t != null).Select(t => t.name).ToList();
        }

        public static bool HasSavedTag(string contestantId, string tag)
        {
            if (gameData == null || string.IsNullOrEmpty(contestantId)) return false;

            TagEntry entry = gameData.tags.FirstOrDefault(
                e => string.Equals(e.contestantId, contestantId, StringComparison.OrdinalIgnoreCase));

            return entry != null && entry.tagNames.Contains(tag);
        }

        public static List<Email> GetEmails()
        {
            return gameData.emails;
        }

        public static void SaveEmails(List<Email> emails)
        {
            gameData.emails = emails;
        }

        #endregion
    }

    [System.Serializable]
    public class ReactionEntry
    {
        public string name;
        public int reaction; // Reaction enum value (-1, 0, 1)
    }

    [System.Serializable]
    public class TagEntry
    {
        public string contestantId;
        public List<string> tagNames = new List<string>();
    }

    [System.Serializable]
    public class GameData
    {
        private const string baseSaveDataPath = "Save Data";

        public string playerName;
        public int moneyAmount;
        public int episodeIndex;
        public string versionNumber;
        public InventoryData inventoryData;
        public List<ReactionEntry> reactions = new List<ReactionEntry>();
        public List<TagEntry> tags = new List<TagEntry>();
        public List<Email> emails = new List<Email>();

        public GameData()
        {
            SaveDataScriptable s = Resources.Load<SaveDataScriptable>(baseSaveDataPath);
            if (s == null)
            {
                Debug.LogError("SaveManager: Save Data not found.");
            }

            playerName = s != null ? s.playerName : "Cindy";
            moneyAmount = s != null ? s.startingMoney : 0;
            episodeIndex = 0;
            versionNumber = Application.version;
            inventoryData = new InventoryData();
        }

        /// <summary>Makes sure nothing is null after JsonUtility loads an older/partial save.</summary>
        public void EnsureInitialized()
        {
            if (inventoryData == null) inventoryData = new InventoryData();
            if (inventoryData.ownedClothingIds == null) inventoryData.ownedClothingIds = new List<string>();
            if (inventoryData.ownedKitIds == null) inventoryData.ownedKitIds = new List<string>();
            if (reactions == null) reactions = new List<ReactionEntry>();
            if (tags == null) tags = new List<TagEntry>();
            if (emails == null) emails = new List<Email>();
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<string> ownedClothingIds = new List<string>();
        public List<string> ownedKitIds = new List<string>();
    }
}
