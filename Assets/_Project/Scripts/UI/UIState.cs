using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mystie.Core;

namespace Mystie.UI
{
    public class UIState : MonoBehaviour
    {
        public event Action onDisplay;
        public event Action onExit;

        protected UIManager manager;

        [SerializeField] protected Button submitBtn;
        [SerializeField] protected Button closeBtn;
        [SerializeField] protected bool closeStateOnCancel = true;
        [SerializeField] public UIState pauseState;

        [Space]

        [SerializeField] private GameObject panel;
        [SerializeField] private List<GameObject> uiElements = new List<GameObject>();
        [SerializeField] protected List<NavButton> navButtons = new List<NavButton>();
        [SerializeField] protected bool showCursor = true;

        [Space]

        [SerializeField] private EventReference displaySFX;
        [SerializeField] private EventReference closeSFX;

        protected virtual void Awake()
        {
            manager = UIManager.Instance;
            if (manager.CurrentState != this)
                CloseState();
        }

        protected virtual void OnEnable()
        {
            if (submitBtn != null) submitBtn.onClick.AddListener(Submit);
            if (closeBtn != null) closeBtn.onClick.AddListener(Close);

            foreach (NavButton navButton in navButtons)
            {
                if (navButton.btn != null) navButton.btn.onClick.AddListener(
                    () => { manager.SetState(navButton.state); });
            }
        }

        protected virtual void OnDisable()
        {
            if (submitBtn != null) submitBtn.onClick.RemoveListener(Submit);
            if (closeBtn != null) closeBtn.onClick.RemoveListener(Close);

            foreach (NavButton navButton in navButtons)
            {
                if (navButton.btn != null) navButton.btn.onClick.RemoveListener(
                    () => { manager.SetState(navButton.state); });
            }
        }

        public virtual void DisplayState()
        {
            if (panel != null) panel.SetActive(true);

            if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    element.SetActive(true);

            Cursor.visible = showCursor;

            if (!displaySFX.IsNull) 
                RuntimeManager.PlayOneShot(displaySFX);

            onDisplay?.Invoke();
        }

        public virtual void PauseState()
        {
            if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    element.SetActive(false);
        }

        public virtual void CloseState()
        {
            if (panel != null) panel.SetActive(false);

            if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    if (element != null) element.SetActive(false);

            Cursor.visible = !showCursor;

            onExit?.Invoke();
        }

        public virtual void Close()
        {
            if (manager.CurrentState == this)
                manager.CloseState();
        }

        public virtual void Escape()
        {
            if (pauseState != null) Pause();
            else Cancel();
        }

        public virtual void Submit() { }

        public virtual void Cancel()
        {
            if (closeStateOnCancel) manager.CloseState();
        }

        public virtual void Pause()
        {
            if (pauseState != null) manager.SetState(pauseState);
            Debug.Log("Pause (" + gameObject.name + ")");
        }
    }
}
