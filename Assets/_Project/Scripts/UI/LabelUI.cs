using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Mystie
{
    public class LabelUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvas;
        [field: SerializeField] public bool isVisible { get; private set; }
        [SerializeField] private LocalizeStringEvent labelLocalized;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image bg;
        [SerializeField] private Image icon;
        [field: SerializeField] public float fadeTime { get; private set; }

        private void Awake()
        {
            if (isVisible) Show(false);
            else Hide(false);
        }

        public void Set(LocalizedString textLocalized, Color color, Sprite sprite = null)
        {
            if (bg != null) bg.color = color;
            Set(textLocalized, sprite);
        }

        public void Set(LocalizedString textLocalized, Sprite sprite = null)
        {
            labelLocalized.StringReference = textLocalized;
            if (!textLocalized.IsEmpty)
                Set(textLocalized.GetLocalizedString(), sprite);
            else Set(string.Empty);
        }

        public void Set(string text, Sprite sprite = null)
        {
            if (label != null) label.text = text;
            if (icon != null)
            {
                icon.sprite = sprite;
                icon.gameObject.SetActive(sprite != null);
            }

            gameObject.SetActive(true);
        }

        public void Show(bool fade = true)
        {
            if (canvas != null)
            {
                if (fade) canvas.DOFade(1, fadeTime);
                else canvas.alpha = 1;
            }
            else
            {
                gameObject.SetActive(true);
            }
            isVisible = true;
        }

        public void ShowForDuration(float duration)
        {
            StartCoroutine(ShowForDurationCoroutine(duration));
        }

        public IEnumerator ShowForDurationCoroutine(float duration)
        {
            Show();

            yield return new WaitForSeconds(duration);

            Hide();
        }

        public void Hide(bool fade = true)
        {
            if (canvas != null)
            {
                if (fade) canvas.DOFade(0, fadeTime);
                else canvas.alpha = 0;
            }
            else
            {
                gameObject.SetActive(false);
            }
            isVisible = false;
        }

        public void Activate()
        {
            gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            canvas = GetComponent<CanvasGroup>();
            label = GetComponentInChildren<TextMeshProUGUI>();
            bg = GetComponentInChildren<Image>();
        }
    }
}
