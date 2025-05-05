using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ItemUI : MonoBehaviour
    {
        public event Action<GarmentScriptable> onSelect;

        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [field: SerializeField] public GarmentScriptable garment { get; private set; }

        public List<ClothingTag> Tags
        {
            get => garment != null ? garment.tags : new List<ClothingTag>();
        }

        private bool init = false;

        private void Awake()
        {
            if (button != null) button.onClick.AddListener(OnSelect);
        }

        private void Start()
        {
            if (!init) Set(garment);
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
            image.enabled = true;
        }

        public void SetEmpty()
        {
            image.sprite = null;
            image.enabled = false;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Set(GarmentScriptable garment)
        {
            this.garment = garment;
            if (image != null && garment != null)
                SetSprite(garment.icon);
            else SetEmpty();
            init = true;
        }

        public void OnSelect()
        {
            onSelect?.Invoke(garment);
        }

        public void Show(bool show)
        {
            if (show)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        public void OnValidate()
        {
            Set(garment);
        }
    }
}
