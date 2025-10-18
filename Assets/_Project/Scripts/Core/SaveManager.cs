using System;
using System.Collections.Generic;
using System.IO;
using Mystie.Core;
using UnityEngine;

namespace Mystie
{
    public class SaveManager
    {
        private string savePath => Application.persistentDataPath + "/savefile.json";
        public GameData gameData { get; private set; }
        public List<GameData> loadedSaveFiles { get; private set; }

        public SaveManager()
        {
            loadedSaveFiles = new List<GameData>();
        }

        public void NewGame()
        {
            gameData = new GameData();
            Debug.Log("SaveManager: New game started.");
        }

        public bool HasSave()
        {
            return !loadedSaveFiles.IsNullOrEmpty();
        }

        public GameData GetSave()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                return gameData;
            }
            else
            {
                Debug.Log("SaveManager: Game data does not exist.");
                return null;
            }
        }

        public void LoadSaveFiles()
        {
            loadedSaveFiles = new List<GameData>();
            GameData save = GetSave();
            if (save != null) loadedSaveFiles.Add(save);
            Debug.Log("SaveManager: Save files loaded.");
        }

        public void LoadGame()
        {
            if (!loadedSaveFiles.IsNullOrEmpty() && loadedSaveFiles.Count > 0)
            {
                gameData = loadedSaveFiles[0];

                GameManager.playerName = gameData.playerName;
                EpisodeManager.Instance.SetEpisodeIndex(gameData.episodeIndex);


                Debug.Log("SaveManager: Game loaded!");
            }
            else
            {
                Debug.Log("SaveManager: No save file found.");
                NewGame();
            }
        }

        public void SaveGame()
        {
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("SaveManager: Game saved to: " + savePath);
        }
    }

    [Serializable]
    public class GameData
    {
        public string playerName = "Cindy";
        public int moneyAmount = 0;
        public int episodeIndex = 0;
        public string versionNumber = Application.version;
    }
}
