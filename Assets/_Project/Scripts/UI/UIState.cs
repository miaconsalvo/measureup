using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mystie.Core;
using DG.Tweening;
using UnityEngine.Serialization;
using NaughtyAttributes;

namespace Mystie.UI
{
    public class UIState : MonoBehaviour
    {
        public event Action onDisplay;
        public event Action onExit;
        public event Action onSubmit;
        public event Action onCancel;

        [SerializeField] private RectTransform rect;

        protected UIManager manager;

        [SerializeField] protected Button submitBtn;
        [SerializeField] protected Button closeBtn;
        [SerializeField] protected bool closeStateOnSubmit = false;
        [SerializeField] protected bool closeStateOnCancel = true;
        [SerializeField] protected UIState submitPopup;
        [SerializeField] public UIState pauseState;

        [Space]

        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private CanvasGroup canvasBackground;
        [SerializeField][Min(0)] private float fadeOutTime = 0.05f;
        [SerializeField][Min(0)] private float fadeInTime = 0.25f;

        [Space]

        [SerializeField] protected List<NavButton> navButtons = new List<NavButton>();
        [SerializeField] protected bool showCursor = true;

        [Space]

        [SerializeField] private EventReference displaySFX;
        [SerializeField] private EventReference closeSFX;

        protected virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
            manager = UIManager.Instance;
            if (canvas != null)
            {
                canvas.alpha = 0;
                canvas.blocksRaycasts = false;
            }

            if (canvasBackground != null)
            {
                canvasBackground.alpha = 0;
                canvasBackground.blocksRaycasts = false;
            }

            if (manager.CurrentState != this)
                CloseState();
        }

        protected virtual void OnEnable()
        {
            if (submitBtn != null) submitBtn.onClick.AddListener(Submit);
            if (closeBtn != null) closeBtn.onClick.AddListener(Close);
            if (submitPopup != null) submitPopup.onSubmit += OnSubmit;

            foreach (NavButton navButton in navButtons)
            {
                navButton.Sub(manager);
            }
        }

        protected virtual void OnDisable()
        {
            if (submitBtn != null) submitBtn.onClick.RemoveListener(Submit);
            if (closeBtn != null) closeBtn.onClick.RemoveListener(Close);
            if (submitPopup != null) submitPopup.onSubmit -= OnSubmit;

            foreach (NavButton navButton in navButtons)
            {
                navButton.Unsub();
            }
        }

        public virtual void DisplayState()
        {
            if (canvas != null)
            {
                canvas.alpha = 1f;
                //canvas.DOFade(1, fadeInTime);
                canvas.blocksRaycasts = true;
            }

            if (canvasBackground != null)
            {
                canvasBackground.alpha = 1f;
                //canvasFocused.DOFade(1, fadeInTime);
                canvasBackground.blocksRaycasts = true;
            }

            Cursor.visible = showCursor;

            if (!displaySFX.IsNull)
                RuntimeManager.PlayOneShot(displaySFX);

            onDisplay?.Invoke();
        }

        public virtual void PauseState()
        {
            if (canvas != null)
            {
                canvas.alpha = 0;
                //canvasFocused.DOFade(0, fadeOutTime);
                canvas.blocksRaycasts = true;
            }
        }

        public virtual void CloseState()
        {
            if (canvas != null)
            {
                canvas.alpha = 0;
                //canvas.DOFade(0, fadeOutTime);
                canvas.blocksRaycasts = false;
            }

            if (canvasBackground != null)
            {
                canvasBackground.alpha = 0;
                //canvasFocused.DOFade(0, fadeOutTime);
                canvasBackground.blocksRaycasts = false;
            }

            if (!closeSFX.IsNull)
                RuntimeManager.PlayOneShot(closeSFX);

            //Debug.Log("Close state : " + gameObject.name);

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

        public virtual void Submit()
        {
            if (submitPopup != null)
            {
                manager.SetState(submitPopup);
            }
            else OnSubmit();
        }

        public virtual void OnSubmit()
        {
            if (closeStateOnSubmit) manager.CloseState();
            onSubmit?.Invoke();
        }

        public virtual void Cancel()
        {
            if (closeStateOnCancel) manager.CloseState();
            onCancel?.Invoke();
        }

        public virtual void Pause()
        {
            if (pauseState != null) manager.SetState(pauseState);
            Debug.Log("Pause (" + gameObject.name + ")");
        }

        [Button]
        public void Show()
        {
            if (canvasBackground != null)
            {
                canvasBackground.alpha = 1f;
                canvasBackground.blocksRaycasts = true;
            }

            if (canvas != null)
            {
                canvas.alpha = 1f;
                canvas.blocksRaycasts = true;
            }
        }

        [Button]
        public void Hide()
        {
            if (canvasBackground != null)
            {
                canvasBackground.alpha = 0;
                canvasBackground.blocksRaycasts = false;
            }

            if (canvas != null)
            {
                canvas.alpha = 0;
                canvas.blocksRaycasts = false;
            }
        }
    }
}
