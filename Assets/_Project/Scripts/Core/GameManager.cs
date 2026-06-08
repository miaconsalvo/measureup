using MeasureUp.Core;
using Mystie.Dressup;
using Mystie.UI.Transition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.SceneManagement;
using Yarn.Unity;

namespace Mystie.Core
{
    public class GameManager
    {
        public static event Action<GameState> onGameStateChanged;
        public static event Action onPause;
        public static event Action onUnpause;

        public GameSettings gameSettings { get; private set; }
        public SystemDataScriptable systemData { get; private set; }
        public SceneTransitioner sceneTransitioner { get; private set; }
        public InputActionAsset actions { get; private set; }
        public Controls controls { get; private set; }
        //public SmartFormatter stringFormatter { get; private set; }

        public static bool isPaused = false;
        public static GameState gameState { get; private set; }

        public const string systemDataPath = "System Data";

        public static string playerName = "Cindy";
        [YarnFunction("playername")]
        public static string Playername() { return playerName; }

        #region Singleton

        protected static GameManager instance = new();
        public static GameManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        #endregion


        private GameManager()
        {
            //DontDestroyOnLoad(gameObject);

            //gameState = GameState.StartScreen;

            isPaused = false;

            systemData = Resources.Load<SystemDataScriptable>(systemDataPath);
            if (systemData == null) Debug.LogError("GameManager: System Data not found.");

            gameSettings = new GameSettings(systemData);

            controls = new Controls();
            //controls.UI.Enable();
            actions = systemData.actions;

            sceneTransitioner = SceneTransitioner.Instance;

            InventoryManager.LoadClothingData();
            SaveDataManager.LoadGameData(new GameData());
            SaveManager.LoadSaveFiles();



            //Cursor.visible = false;
            //cursor = Instantiate(systemData.cursorPrefab);



            //SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log("Game Manager: Instantiated");
        }

        public static void SetGameState(GameState state)
        {
            if (gameState != state) return;

            gameState = state;
            onGameStateChanged?.Invoke(gameState);
        }

        public static void Pause()
        {
            if (isPaused) return;

            gameState = GameState.Pause;

            Time.timeScale = 0f;
            isPaused = true;
            onPause?.Invoke();
        }

        public static void Unpause()
        {
            if (!isPaused) return;

            gameState = GameState.Play;

            Time.timeScale = 1f;
            isPaused = false;
            onUnpause?.Invoke();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Unpause();
        }

        public void LoadMainMenu()
        {
            Debug.Log("Loading main menu...");
            gameState = GameState.MainMenu;
            SceneManager.LoadSceneAsync(systemData.mainMenuScene);
        }

        public void Gameover()
        {
            Debug.Log("Loading gameover...");
            gameState = GameState.Gameover;
            SceneManager.LoadSceneAsync(systemData.gameoverScene);
        }

        public static void Quit()
        {
            Debug.Log("Quitting the game...");

#if UNITY_EDITOR

            if (UnityEditor.EditorApplication.isPlaying == true)
                UnityEditor.EditorApplication.isPlaying = false;

#endif

            Application.Quit();
        }
    }

    public enum GameState
    {
        StartScreen,
        MainMenu,
        Play,
        Pause,
        Gameover
    }
}
