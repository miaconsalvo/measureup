using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ItemUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<ItemScriptable> onSelect;
        public event Action<ItemScriptable> onEquipped;
        public event Action<ItemScriptable> onUnequipped;

        private DragAndDropHandler dragAndDropHandler;

        [field: SerializeField] public ItemScriptable item { get; private set; }
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        private RectTransform t;

        [Space]

        [SerializeField] private Color buttonColor;
        [SerializeField] private Color buttonColorSelected;
        [SerializeField] private Color iconColor;
        [SerializeField] private Color iconColorSelected;

        [SerializeField] private bool isEquipped;
        [SerializeField] private bool isSelectableEquip;
        [SerializeField] private bool isSelectableUnequip;
        [SerializeField] private bool isDraggable;

        public List<ClothingTag> Tags
        {
            get => item != null ? item.tags : new List<ClothingTag>();
        }

        private bool init = false;

        private void Awake()
        {
            t = GetComponent<RectTransform>();
            if (button != null) button.onClick.AddListener(OnSelect);
            isEquipped = false;
        }

        private void Start()
        {
            if (!init) Set(item);
            dragAndDropHandler = DragAndDropHandler.Instance;
        }

        public void Init(ItemScriptable item, Action<ItemScriptable> onSelect, Action<ItemScriptable> onEquipped, Action<ItemScriptable> onUnequipped)
        {
            this.onSelect = onSelect;
            this.onEquipped = onEquipped;
            this.onUnequipped = onUnequipped;
            Set(item);
        }

        public void Set(ItemScriptable item)
        {
            this.item = item;
            if (image != null && item != null)
                SetSprite(item.icon);
            else SetEmpty();

            SetEquipped(isEquipped);

            init = true;
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

        [Button()]
        public void UpdateDisplay()
        {
            Set(item);
        }

        public void OnSelect()
        {
            onSelect?.Invoke(item);

            if (!isEquipped && isSelectableEquip) Equip();
            else if (isEquipped && isSelectableUnequip) Unequip();
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

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        [Button()]
        private void Equip()
        {
            SetEquipped(true);
            onEquipped?.Invoke(item);
        }

        [Button()]
        private void Unequip()
        {
            SetEquipped(false);
            onUnequipped?.Invoke(item);
        }

        private void SetEquipped(bool isEquipped)
        {
            this.isEquipped = isEquipped;

            if (button != null) button.image.color = isEquipped ? buttonColorSelected : buttonColor;
            if (image != null) image.color = isEquipped ? iconColorSelected : iconColor;
        }

        #region Drag and Drop

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isDraggable) return;
            //Debug.Log("Dragging", this);

            dragAndDropHandler.StartDrag(
                this,
                item,
                image.sprite,
                t.sizeDelta,
                DropItem
                );
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDraggable) return;
            dragAndDropHandler.EndDrag(this);
        }

        public void DropItem(object data, GameObject target)
        {
            DressupModelUI modelUI = null;
            ItemScriptable item = data as ItemScriptable;
            if (target == null || data == null
            || (modelUI = target.GetComponent<DressupModelUI>()) == null)
                return;

            Equip();
        }

        #endregion

        public void Reset()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }
    }
}
