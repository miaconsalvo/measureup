using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Mystie.UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class TooltipUI : MonoBehaviour
    {
        public RectTransform rectTransform;
        public CanvasGroup canvas;
        public LayoutElement layoutElement;
        public LayoutGroup layoutGroup;
        public float showFadeDuration = 0.2f;

        public TextMeshProUGUI headerField;
        public TextMeshProUGUI contentField;

        private Tween showTween;

        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Hide();
        }

        public void SetText(string content, string header = "")
        {
            if (headerField != null)
            {
                if (!string.IsNullOrEmpty(header))
                {
                    headerField.gameObject.SetActive(true);
                    headerField.text = header;
                }
                else
                    headerField.gameObject.SetActive(false);
            }

            if (contentField != null)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    contentField.gameObject.SetActive(true);
                    contentField.text = content;
                }
                else
                    contentField.gameObject.SetActive(false);
            }

            UpdateSize();
        }

        private void UpdateSize()
        {
            Canvas.ForceUpdateCanvases();

            if (layoutElement)
            {
                layoutElement.enabled = Math.Max(
                    headerField != null ? headerField.preferredWidth : 0,
                    contentField != null ? contentField.preferredWidth : 0)
                    >= layoutElement.preferredWidth;
            }

            if (layoutGroup)
            {
                layoutGroup.enabled = false;
                layoutGroup.enabled = true;
            }
        }


        [Button]
        public void Show()
        {
            showTween = canvas.DOFade(1, showFadeDuration);
            //canvas.alpha = 1;
            canvas.blocksRaycasts = true;
        }

        [Button]
        public void Hide()
        {
            showTween?.Kill();
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
        }

        public void OnValidate()
        {
            UpdateSize();
        }

        public void Reset()
        {
            canvas = GetComponentInChildren<CanvasGroup>();
            layoutElement = GetComponentInChildren<LayoutElement>();
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length > 0) headerField = texts[0];
            if (texts.Length > 1) contentField = texts[1];
        }
    }
}
