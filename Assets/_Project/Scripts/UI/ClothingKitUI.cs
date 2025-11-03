using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ClothingKitUI : MonoBehaviour
    {
        public event Action<ClothingKitUI, ClothingKitScriptable> onSelect;

        public Button button { get; private set; }
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI priceRangeLabel;
        [SerializeField] private TagsDisplayUI tagsUI;

        [field: SerializeField] public bool isPurchased { get; private set; }

        [Space]

        [SerializeField] private Transform itemsAnchor;
        private List<ItemUI> itemsUI = new List<ItemUI>();

        [field: SerializeField] public ClothingKitScriptable clothingKit { get; private set; }

        private void Awake()
        {
            button = GetComponent<Button>();
            if (button != null) button.onClick.AddListener(OnSelect);

            itemsUI = itemsAnchor.GetComponentsInChildren<ItemUI>().ToList();
            //Set(clothingKit);
            //SetPurchased(isPurchased);
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveAllListeners();
        }

        public void Set(ClothingKitScriptable kit)
        {
            clothingKit = kit;

            if (kit == null)
            {
                Clear();
                return;
            }

            if (nameLabel != null) nameLabel.text = kit.name;
            if (priceLabel != null) priceLabel.text = "$" + String.Format("{0:0.00}", kit.price);
            if (priceRangeLabel != null) priceRangeLabel.text = kit.priceRange;

            if (tagsUI != null) tagsUI.SetTags(kit.tags);

            for (int i = 0; i < itemsUI.Count; i++)
            {
                if (clothingKit != null && i < clothingKit.garments.Count)
                    itemsUI[i].Set(clothingKit.garments[i]);
                else itemsUI[i].SetEmpty();
            }
        }

        public void Clear()
        {
            if (nameLabel != null) nameLabel.text = "None";
            if (priceLabel != null) priceLabel.text = "$0.00";
            if (priceRangeLabel != null) priceRangeLabel.text = "";

            tagsUI.SetTags(null);

            foreach (ItemUI item in itemsUI)
                item.SetEmpty();
        }

        public void SetPurchased(bool purchased = true)
        {
            isPurchased = purchased;
            if (canvas != null)
            {
                if (purchased)
                {
                    canvas.alpha = 0.8f;
                    canvas.interactable = false;
                    if (priceLabel != null) priceLabel.text = "[SOLD]";
                    if (priceRangeLabel != null) priceRangeLabel.text = "[SOLD]";
                }
                else
                {
                    canvas.alpha = 1f;
                    canvas.interactable = true;
                    if (priceLabel != null) priceLabel.text = "$" + String.Format("{0:0.00}", clothingKit.price);
                    if (priceRangeLabel != null) priceRangeLabel.text = clothingKit.priceRange;
                }
            }
            transform.SetAsLastSibling();
        }

        public void OnSelect()
        {
            onSelect?.Invoke(this, clothingKit);
        }

        public void UpdateItems()
        {
            itemsUI = itemsAnchor.GetComponentsInChildren<ItemUI>().ToList();
        }
    }
}
