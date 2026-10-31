using System;
using System.Collections.Generic;
using System.IO;
using Mystie.Core;
using UnityEngine;

namespace Mystie
{
    public static class SaveManager
    {
        private static string savePath => Application.persistentDataPath + "/savefile.json";
        public static List<GameData> loadedSaveFiles { get; private set; } = new List<GameData>();

        public static void NewGame()
        {
            SaveDataManager.LoadGameData(new GameData());
            EpisodeManager.Instance.LoadEpisode(0);
            Debug.Log("SaveManager: New game started.");
        }

        public static bool HasSave()
        {
            return !loadedSaveFiles.IsNullOrEmpty();
        }

        public static GameData GetSave()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    string json = File.ReadAllText(savePath);
                    GameData gameData = JsonUtility.FromJson<GameData>(json);
                    if (gameData != null) gameData.EnsureInitialized();
                    return gameData;
                }

                Debug.Log("SaveManager: Game data does not exist.");
                return null;
            }
            catch (Exception e)
            {
                // Corrupt/unreadable file.
                Debug.LogError("SaveManager: Failed to read save file: " + e);
                return null;
            }
        }

        public static void LoadSaveFiles()
        {
            loadedSaveFiles = new List<GameData>();
            GameData save = GetSave();
            if (save != null) loadedSaveFiles.Add(save);
            Debug.Log("SaveManager: Save files loaded.");
        }

        public static void LoadGame()
        {
            if (!loadedSaveFiles.IsNullOrEmpty() && loadedSaveFiles[0] != null)
            {
                SaveDataManager.LoadGameData(loadedSaveFiles[0]);
                EpisodeManager.Instance.LoadCurrentEpisode();

                Debug.Log("SaveManager: Game loaded!");
            }
            else
            {
                Debug.Log("SaveManager: No save file found.");
                NewGame();
            }
        }

        public static void SaveGameToFile(GameData gameData)
        {
            try
            {
                string json = JsonUtility.ToJson(gameData, true);
                File.WriteAllText(savePath, json);
                Debug.Log("SaveManager: Game saved to: " + savePath);
            }
            catch (Exception e)
            {
                Debug.LogError("SaveManager: Failed to write save file: " + e);
            }
        }
    }
}
