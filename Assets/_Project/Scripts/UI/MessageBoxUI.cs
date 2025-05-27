using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    public class MessageBoxUI : MonoBehaviour
    {
        //[SerializeField] private RectTransform container;
        [SerializeField] private Image bg;
        [field: SerializeField] public TextMeshProUGUI characterName { get; private set; }
        [field: SerializeField] public TextMeshProUGUI text { get; private set; }
        [SerializeField] private Image profilePicture;
        [SerializeField] private HorizontalLayoutGroup layout;
        [SerializeField] private List<GameObject> messageComponents;
        [SerializeField] private GameObject loading;
        [SerializeField] private bool isLoading;

        [SerializeField] private MessageBoxSettings settings;
        private Sprite profilePictureSpriteDefault;

        public void Awake()
        {
            SetLoading(isLoading);
        }

        public void Set(MessageBoxSettings newSettings, Sprite profilePictureSprite = null)
        {
            if (profilePicture != null && profilePictureSpriteDefault == null)
                profilePictureSpriteDefault = profilePicture.sprite;

            settings.Set(newSettings);

            if (profilePicture != null)
                profilePicture.sprite = profilePictureSprite != null ? profilePictureSprite : profilePictureSpriteDefault;

            UpdateDisplay();
        }

        public void SetLoading(bool isLoading)
        {
            this.isLoading = isLoading;
            loading.SetActive(isLoading);
            foreach (GameObject comp in messageComponents)
            {
                comp.SetActive(!isLoading);
            }

            UpdateDisplay();
        }

        public void SetText(string text, string characterName = null)
        {
            if (this.text != null) this.text.text = text;
            if (this.characterName != null)
            {
                this.characterName.text = characterName;
                this.characterName.gameObject.SetActive(settings.showName && !string.IsNullOrWhiteSpace(characterName));
            }
        }

        public void UpdateDisplay()
        {
            if (bg != null) bg.color = settings.bgColor;
            //if (text != null) text.color = textColor;
            if (characterName != null) characterName.gameObject.SetActive(settings.showName);
            if (profilePicture != null) profilePicture.gameObject.SetActive(settings.showProfilePicture);

            if (layout != null)
            {
                layout.reverseArrangement = settings.isAlignedRight;
                layout.childAlignment = settings.isAlignedRight ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
            }
        }

        public void OnValidate()
        {
            UpdateDisplay();
        }
    }

    [Serializable]
    public class MessageBoxSettings
    {
        [field: SerializeField] public Color bgColor { get; private set; } = Color.white;
        [field: SerializeField] public bool isAlignedRight { get; private set; } = false;
        [field: SerializeField] public bool showName { get; private set; } = true;
        [field: SerializeField] public bool showProfilePicture { get; private set; } = true;

        public void Set(MessageBoxSettings settings)
        {
            Set(settings.bgColor, settings.isAlignedRight, settings.showName, settings.showProfilePicture);
        }

        public void Set(Color bgColor, bool isAlignedRight = false, bool showName = true, bool showProfilePicture = true)
        {
            this.bgColor = bgColor;
            //this.textColor = textColor;
            this.showName = showName;
            this.isAlignedRight = isAlignedRight;
            this.showProfilePicture = showProfilePicture;
        }
    }
}
