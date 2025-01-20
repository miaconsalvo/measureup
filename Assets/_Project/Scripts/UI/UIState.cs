using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mystie.Core;
using DG.Tweening;

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

        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private CanvasGroup canvasFocused;
        [SerializeField][Min(0)] private float fadeOutTime = 0.05f;
        [SerializeField][Min(0)] private float fadeInTime = 0.25f;

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
            if (canvas != null) {
                canvas.alpha = 0;
                canvas.blocksRaycasts = false;
            } 

            if (canvasFocused != null) {
                canvasFocused.alpha = 0;
                canvasFocused.blocksRaycasts = false;
            } 

            if (manager.CurrentState != this)
                CloseState();
        }

        protected virtual void OnEnable()
        {
            if (submitBtn != null) submitBtn.onClick.AddListener(Submit);
            if (closeBtn != null) closeBtn.onClick.AddListener(Close);

            foreach (NavButton navButton in navButtons) {
                navButton.Sub(manager);
            }
        }

        protected virtual void OnDisable()
        {
            if (submitBtn != null) submitBtn.onClick.RemoveListener(Submit);
            if (closeBtn != null) closeBtn.onClick.RemoveListener(Close);

            foreach (NavButton navButton in navButtons) {
                navButton.Unsub();
            }
        }

        public virtual void DisplayState()
        {
            if (canvas != null){
                canvas.alpha = 1f;
                //canvas.DOFade(1, fadeInTime);
                canvas.blocksRaycasts = true;
            }

            if (canvasFocused != null){
                canvasFocused.alpha = 1f;
                //canvasFocused.DOFade(1, fadeInTime);
                canvasFocused.blocksRaycasts = true;
            }

            /*if (panel != null) panel.SetActive(true);

            if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    element.SetActive(true);*/

            Cursor.visible = showCursor;

            if (!displaySFX.IsNull) 
                RuntimeManager.PlayOneShot(displaySFX);

            onDisplay?.Invoke();
        }

        public virtual void PauseState()
        {
            if(canvasFocused != null){
                canvasFocused.alpha = 0;
                //canvasFocused.DOFade(0, fadeOutTime);
                canvasFocused.blocksRaycasts = true;
            }

            /*if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    element.SetActive(false);*/
        }

        public virtual void CloseState()
        {
            if (canvas != null){
                canvas.alpha = 0;
                //canvas.DOFade(0, fadeOutTime);
                canvas.blocksRaycasts = false;
            }
            
            if (canvasFocused != null){
                canvasFocused.alpha = 0;
                //canvasFocused.DOFade(0, fadeOutTime);
                canvasFocused.blocksRaycasts = false;
            }

            /*if (panel != null) panel.SetActive(false);

            if (!uiElements.IsNullOrEmpty())
                foreach (GameObject element in uiElements)
                    if (element != null) element.SetActive(false);

            //Cursor.visible = !showCursor;*/

            if (!displaySFX.IsNull) 
                RuntimeManager.PlayOneShot(closeSFX);

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
