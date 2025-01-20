using Mystie.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        //private GameManager gameManager;

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

        [SerializeField] private Vector2 refResolution = new Vector2(1920, 1080);

        public static Vector2 RefResolution { get { return Instance.refResolution; } }
        public static float ScreenScale => Screen.width / RefResolution.x;

        public static Controls controls;

        [SerializeField] public List<UIState> startStates = new List<UIState>();
        protected Stack<UIState> stateStack = new Stack<UIState>();

        public UIState CurrentState { get { return (stateStack.Count > 0) ? stateStack.Peek() : null; } }

        void Awake()
        {
            //If an instance already exists
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            //gameManager = GameManager.Instance;
            //controls = gameManager.controls;
            controls = new Controls();
            controls.UI.Enable();
        }

        protected void OnEnable()
        {
            if (controls != null)
            {
                controls.UI.Submit.performed += ctx => { CurrentState?.Submit(); };
                controls.UI.Cancel.performed += ctx => { CurrentState?.Cancel(); };
                controls.UI.Pause.performed += ctx => { CurrentState?.Pause(); };
                //controls.UI.Escape.performed += ctx => { CurrentState?.Escape(); };
            }
        }

        protected void OnDisable()
        {
            if (controls != null)
            {
                controls.UI.Submit.performed -= ctx => { CurrentState?.Submit(); };
                controls.UI.Cancel.performed -= ctx => { CurrentState?.Cancel(); };
                controls.UI.Pause.performed -= ctx => { CurrentState?.Pause(); };
                //controls.UI.Escape.performed -= ctx => { CurrentState?.Escape(); };
            }
              
            GameManager.Unpause();
        }

        public void Start()
        {
            foreach(UIState state in startStates)
                if (state != null) SetState(state);
        }

        public void SetState(UIState newState)
        {
            if (newState == null) return;

            if (CurrentState != null) CurrentState.PauseState(); // pause the current state

            newState.DisplayState();

            stateStack.Push(newState); // we push the new state on top of the stack
        }

        public void CloseState()
        {
            if (CurrentState == null) return;

            stateStack.Pop().CloseState(); // we close the current state
            if (CurrentState != null) CurrentState?.DisplayState();
        }

        public void ClearStates()
        {
            while (CurrentState != null) CloseState();
        }
    }
}
