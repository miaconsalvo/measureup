using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Mystie
{
    public class LabelUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent labelLocalized;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image bg;
        [SerializeField] private Image icon;

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

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            label = GetComponentInChildren<TextMeshProUGUI>();
            bg = GetComponentInChildren<Image>();
        }
    }
}
