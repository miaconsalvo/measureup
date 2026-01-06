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
using VInspector;
using UnityEngine.Localization;

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

        [Space]

        [SerializeField] protected LocalizedString submitPopupText;
        [SerializeField] protected LocalizedString closePopupText;

        private Tween canvasTween, canvasBGTween;

        protected virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
            manager = UIManager.Instance;

            Hide();
        }

        protected virtual void OnEnable()
        {
            if (submitBtn != null) submitBtn.onClick.AddListener(Submit);
            if (closeBtn != null) closeBtn.onClick.AddListener(CloseState);

            foreach (NavButton navButton in navButtons)
            {
                navButton.Sub(manager);
            }
        }

        protected virtual void OnDisable()
        {
            if (submitBtn != null) submitBtn.onClick.RemoveListener(Submit);
            if (closeBtn != null) closeBtn.onClick.RemoveListener(CloseState);

            foreach (NavButton navButton in navButtons)
            {
                navButton.Unsub();
            }
        }

        public virtual IEnumerator DisplayState()
        {
            StopCoroutine(HideState());

            if (canvas != null)
                canvasTween = canvas.DOFade(1, fadeInTime).SetUpdate(true);

            if (canvasBackground != null)
                canvasBGTween = canvasBackground.DOFade(1, fadeInTime).SetUpdate(true);

            yield return new WaitForSecondsRealtime(fadeInTime);

            if (canvas != null) canvas.blocksRaycasts = true;
            if (canvasBackground != null) canvasBackground.blocksRaycasts = true;

            if (rect != null) LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

            Cursor.visible = showCursor;

            if (!displaySFX.IsNull)
                RuntimeManager.PlayOneShot(displaySFX);

            onDisplay?.Invoke();

            yield break;
        }

        public virtual void ResumeState()
        {
            StopCoroutine(HideState());
            canvasTween?.Kill();
            canvasBGTween?.Kill();

            if (canvas != null)
            {
                canvas.alpha = 1;
                canvas.blocksRaycasts = true;
            }
        }

        public virtual void PauseState()
        {
            StopCoroutine(DisplayState());
            canvasTween?.Kill();
            canvasBGTween?.Kill();

            if (canvas != null)
            {
                canvas.alpha = 0;
                canvas.blocksRaycasts = false;
            }
        }

        public virtual IEnumerator HideState(bool immediate = false)
        {
            StopCoroutine(DisplayState());
            canvasTween?.Kill();
            canvasBGTween?.Kill();

            if (immediate)
            {
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
            }
            else
            {
                if (canvas != null)
                {
                    canvasTween = canvas.DOFade(0, fadeOutTime).SetUpdate(true);
                    canvas.blocksRaycasts = false;
                }

                if (canvasBackground != null)
                {
                    canvasBGTween = canvasBackground.DOFade(0, fadeOutTime).SetUpdate(true);
                    canvasBackground.blocksRaycasts = false;
                }

                if (!closeSFX.IsNull)
                    RuntimeManager.PlayOneShot(closeSFX);

                yield return new WaitForSecondsRealtime(fadeOutTime);
            }

            onExit?.Invoke();

            yield break;
        }

        public virtual void SetState()
        {
            if (manager.CurrentState != this)
                manager.SetState(this);
        }

        public virtual void CloseState()
        {
            if (manager.CurrentState != this) return;

            if (!closePopupText.IsEmpty)
                PopupEvents.RequestConfirmation(closePopupText, OnClose);
            else OnClose();
        }

        public virtual void Escape()
        {
            if (pauseState != null) Pause();
            else Cancel();
        }

        public virtual void Submit()
        {
            if (manager.CurrentState != this) return;

            if (!submitPopupText.IsEmpty)
                PopupEvents.RequestConfirmation(submitPopupText, OnSubmit);
            else OnSubmit();
        }

        public virtual void OnSubmit()
        {
            onSubmit?.Invoke();
            if (closeStateOnSubmit) manager.CloseState();
        }

        public virtual void OnClose()
        {
            manager.CloseState();
        }

        public virtual void Cancel()
        {
            onCancel?.Invoke();
            if (closeStateOnCancel) manager.CloseState();
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
