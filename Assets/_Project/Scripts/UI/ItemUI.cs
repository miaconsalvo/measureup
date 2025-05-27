using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ItemUI : MonoBehaviour
    {
        public event Action<ItemScriptable, bool> onSelect;

        private DragAndDropHandler dragAndDropHandler;

        [field: SerializeField] public ItemScriptable item { get; private set; }
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        [Space]

        [SerializeField] private Color buttonColor;
        [SerializeField] private Color buttonColorSelected;
        [SerializeField] private Color iconColor;
        [SerializeField] private Color iconColorSelected;

        [SerializeField] private bool isSelected;

        public List<ClothingTag> Tags
        {
            get => item != null ? item.tags : new List<ClothingTag>();
        }

        private bool init = false;

        private void Awake()
        {
            if (button != null) button.onClick.AddListener(OnSelect);
            isSelected = false;
        }

        private void Start()
        {
            if (!init) Set(item);
            dragAndDropHandler = DragAndDropHandler.Instance;
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

        public void Set(ItemScriptable item)
        {
            this.item = item;
            if (image != null && item != null)
                SetSprite(item.icon);
            else SetEmpty();

            SetSelected(isSelected);

            init = true;
        }

        [Button()]
        public void UpdateDisplay()
        {
            Set(item);
        }

        public void OnSelect()
        {
            SetSelected(!isSelected);
            onSelect?.Invoke(item, isSelected);
        }

        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;

            if (button != null) button.image.color = isSelected ? buttonColorSelected : buttonColor;
            if (image != null) image.color = isSelected ? iconColorSelected : iconColor;
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

        #region Drag and Drop

        public void OnBeginDrag(PointerEventData pointer)
        {

        }

        public void OnDrag(PointerEventData pointer)
        {

        }

        public void OnEndDrag(PointerEventData pointer)
        {

        }

        #endregion

        public void Reset()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        public void Oalidate()
        {
            SetSelected(isSelected);
        }
    }
}
