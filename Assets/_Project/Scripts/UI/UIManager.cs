using MeasureUp.Core;
using Mystie.Core;
using Mystie.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace Mystie.UI
{
    public class UIManager : MonoBehaviour
    {
        private GameManager gameManager;

        #region Singleton

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<UIManager>();
                return instance;
            }
        }

        protected static UIManager instance;

        #endregion

        [SerializeField] private RectTransform frame;
        [SerializeField] private Vector2 refResolution = new Vector2(1920, 1080);

        public static Vector2 RefResolution { get { return Instance.refResolution; } }
        public static float ScreenScale => Screen.width / RefResolution.x;

        public static Controls controls;

        [SerializeField] public float startDelay;
        [SerializeField] public List<UIState> startStates = new List<UIState>();
        protected Stack<UIState> stateStack = new Stack<UIState>();

        public UIState CurrentState => (stateStack.Count > 0) ? stateStack.Peek() : null;

        private float lastStateChangeTime = 0f;
        private const float INPUT_BUFFER_TIME = 0.1f; // 100ms buffer
        private bool IsInputBuffer { get => Time.unscaledTime - lastStateChangeTime >= INPUT_BUFFER_TIME; }

        [SerializeField] private bool showDebug;

        void Awake()
        {
            //If an instance already exists
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            gameManager = GameManager.Instance;
            controls = gameManager.controls;
            //controls = new Controls();
        }

        protected void OnEnable()
        {
            controls.UI.Enable();
            if (controls != null)
            {
                controls.UI.Submit.started += OnSubmit;
                controls.UI.Cancel.started += OnCancel;
                controls.UI.Pause.started += OnPause;
                //controls.UI.Escape.performed += ctx => { CurrentState?.Escape(); };
            }
        }

        protected void OnDisable()
        {
            controls.UI.Disable();
            if (controls != null)
            {
                controls.UI.Submit.started -= OnSubmit;
                controls.UI.Cancel.started -= OnCancel;
                controls.UI.Pause.started -= OnPause;
                //controls.UI.Escape.performed -= ctx => { CurrentState?.Escape(); };
            }

            GameManager.Unpause();
        }

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(startDelay);

            foreach (UIState state in startStates)
                if (state != null) SetState(state);
        }

        public void OnStateChange()
        {
            lastStateChangeTime = Time.unscaledTime;
        }

        public void SetState(UIState newState)
        {
            if (newState == null) return;

            if (CurrentState != null)
            {
                if (showDebug) Debug.Log("UIManager: Paused state: " + CurrentState.name);
                CurrentState.PauseState(); // pause the current state
            }
            if (showDebug) Debug.Log("UIManager: Set state: " + newState.name);

            OnStateChange();
            stateStack.Push(newState); // we push the new state on top of the stack
            StartCoroutine(newState.DisplayState());
        }

        public void CloseState(bool immediate = false)
        {
            if (CurrentState == null) return;

            if (showDebug) Debug.Log("UIManager: Closing State " + CurrentState.name);

            OnStateChange();

            StartCoroutine(stateStack.Pop().HideState(immediate)); // we close the current state
            if (CurrentState != null) CurrentState?.ResumeState();
        }

        public void ClearStates()
        {
            if (showDebug) Debug.Log("UIManager: Clear all states");
            while (CurrentState != null) CloseState(true);
        }

        public void OnSubmit(CallbackContext ctx) { if (IsInputBuffer) CurrentState?.Submit(); }

        public void OnCancel(CallbackContext ctx) { if (IsInputBuffer) CurrentState?.Cancel(); }

        public void OnEscape() { if (IsInputBuffer) CurrentState?.Escape(); }

        public void OnPause(CallbackContext ctx) { CurrentState?.Pause(); }

        public string GetStateStackString()
        {
            string s = "UIManager: State Stack\n";

            foreach (UIState state in stateStack)
            {
                s += "\n\tUI State: " + state.gameObject.name;
            }

            return s;
        }

#if UNITY_EDITOR
        protected virtual void OnGUI()
        {
            if (showDebug && Selection.Contains(gameObject))
            {
                GUIStyle style = new GUIStyle();
                float ratio = UIManager.ScreenScale;
                style.fontSize = (int)Math.Ceiling(24 * ratio);
                style.normal.textColor = Color.black;

                Vector2 size = new Vector2(200f, 200f) * ratio;
                Vector2 offset = new Vector2(48f, 48f) * ratio;
                Vector2 gap = new Vector2(0f, 24f) * ratio;

                //RectData rect = new RectData(200f, 40f, Screen.width - (300f), Screen.height - 90f, 0f, 50f);
                //RectData rect = new RectData(size.x, size.y, Screen.width - size.x - offset.x, Screen.height - size.y - offset.y, gap.x, gap.y);
                RectData rect = new RectData(size.x, size.y, offset.x, offset.y, gap.x, gap.y);

                //string displayText = "UI State: " + (CurrentState != null ? CurrentState.name : "null");
                string displayText = GetStateStackString();

                GUI.Label(rect.GetRect(0), displayText, style);
            }
        }
#endif
    }
}
