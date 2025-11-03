using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mystie.Dressup;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Mystie
{
    public class ItemDetailsPanelUI : MonoBehaviour
    {
        private Action<Locale> UpdateLocale;

        [field: SerializeField] public ItemScriptable item { get; private set; }

        [Space]

        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI brandText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Image brandImage;
        [SerializeField] private List<TagsDisplayUI> tagsUI = new List<TagsDisplayUI>();
        [field: SerializeField] public float fadeTime { get; private set; } = 0.3f;

        void Awake()
        {
            UpdateLocale = (Locale locale) => { UpdateDisplay(); };
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateLocale;
            UpdateDisplay();
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateLocale;
        }

        public void Set(ItemScriptable newItem)
        {
            item = newItem;
            UpdateDisplay();
        }

        [Button()]
        public void UpdateDisplay()
        {
            if (item == null)
            {
                Clear();
                return;
            }

            if (nameText != null) nameText.text = item.name;
            if (brandText != null)
            {
                brandText.gameObject.SetActive(item.brand != null);
                if (item.brand != null) brandText.text = "<b>Brand:</b> " + item.brand.name;
            }

            if (descriptionText != null)
            {
                if (!item.description.IsEmpty) descriptionText.text = item.description.GetLocalizedString();
                descriptionText.gameObject.SetActive(!item.description.IsEmpty);
            }
            if (itemImage != null)
            {
                itemImage.sprite = item.icon;
                itemImage.gameObject.SetActive(item.icon != null);
            }
            if (brandImage != null)
            {
                if (item.brand != null)
                {
                    brandImage.sprite = item.brand.logo;
                    brandImage.gameObject.SetActive(item.brand.logo != null);
                }
                else
                {
                    brandImage.sprite = null;
                    brandImage.gameObject.SetActive(false);
                }
            }

            foreach (TagsDisplayUI ui in tagsUI) ui.SetTags(item.tags);
        }

        [Button()]
        public void Clear()
        {
            if (nameText != null) nameText.text = "None";
            if (brandText != null) brandText.gameObject.SetActive(false);
            if (descriptionText != null)
            {
                descriptionText.text = null;
                descriptionText.gameObject.SetActive(false);
            }
            if (itemImage != null)
            {
                itemImage.sprite = null;
                itemImage.gameObject.SetActive(false);
            }

            foreach (TagsDisplayUI ui in tagsUI) ui.SetTags(null);
        }

        public void Show(bool fade = true)
        {
            if (fade) canvas.DOFade(1, fadeTime);
            else canvas.alpha = 1;
        }

        public void Hide(bool fade = true)
        {
            if (fade) canvas.DOFade(0, fadeTime);
            else canvas.alpha = 0;
        }
    }
}
