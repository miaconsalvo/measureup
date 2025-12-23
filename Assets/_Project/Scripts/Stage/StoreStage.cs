using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using Mystie.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class StoreStage : LevelStage
    {
        public InventoryManager inventory { get; private set; }
        [SerializeField] private ClothingKitUI detailsPanel;
        [SerializeField] private Transform kitPreviewsAnchor;
        [SerializeField] private ClothingKitUI kitPreviewPrefab;
        private ClothingKitUI selectedKitUI;

        private List<ClothingKitUI> kitUIs;

        [Space]

        [SerializeField] private Button purchaseButton;
        [SerializeField] private LabelUI moneyUI;
        //[SerializeField] private int money;

        [Space]

        [SerializeField] private LocalizedString purchaseConfirmText;

        private List<ClothingKitScriptable> kits;

        protected override void Awake()
        {
            base.Awake();

            inventory = levelManager.inventory;
            inventory.onMoneyUpdated += UpdateMoneyUI;

            purchaseButton.onClick.AddListener(OnPurchase);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            inventory.onMoneyUpdated -= UpdateMoneyUI;

            purchaseButton.onClick.RemoveListener(OnPurchase);
        }

        public void Initialize(EpisodeScriptable episode)
        {
            kits = episode.kitsOnSale;
            // Todo Disable owned kits on start
            kitUIs = new List<ClothingKitUI>();
            foreach (Transform child in kitPreviewsAnchor)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (ClothingKitScriptable kit in kits)
            {
                ClothingKitUI clothingKitUI = Instantiate(kitPreviewPrefab.gameObject, kitPreviewsAnchor).GetComponent<ClothingKitUI>();
                clothingKitUI.Set(kit);
                clothingKitUI.SetPurchased(false);
                clothingKitUI.onSelect += OnKitSelected;
                kitUIs.Add(clothingKitUI);
            }

            foreach (ClothingKitUI kitUI in kitUIs)
            {
                Debug.Log($"Kit ui: {kitUI.clothingKit}. Owned: {inventory.ownedKits.Contains(kitUI.clothingKit)}");
                if (inventory.ownedKits.Contains(kitUI.clothingKit))
                {
                    kitUI.SetPurchased();
                }
            }

            detailsPanel.gameObject.SetActive(false);
            UpdateMoneyUI(inventory.moneyAmount);

            completeStageButton.interactable = inventory.ownedKits.Count > 0;
        }

        private void OnKitSelected(ClothingKitUI kitUI, ClothingKitScriptable kit)
        {
            selectedKitUI = kitUI;
            if (detailsPanel != null)
            {
                detailsPanel.gameObject.SetActive(true);
                detailsPanel.Set(kit);
            }

            detailsPanel.SetPurchased(!kitUI.isPurchased);
            purchaseButton.interactable = CanBuy(kit.price);
        }

        private void OnKitDeselect()
        {
            if (detailsPanel != null)
            {
                detailsPanel.gameObject.SetActive(false);
            }
        }

        public void GainMoney(int amount)
        {
            inventory.GainMoney(amount);
        }

        public bool CanBuy(int price)
        {
            return price <= inventory.moneyAmount;
        }

        public bool Purchase(int price)
        {
            if (!CanBuy(price)) return false;

            inventory.GainMoney(-price);

            return true;
        }

        public void OnPurchase()
        {
            PopupEvents.RequestConfirmation(purchaseConfirmText, OnConfirmPurchase);
        }

        public void OnConfirmPurchase()
        {
            if (Purchase(selectedKitUI.clothingKit.price))
            {
                detailsPanel.SetPurchased();
                selectedKitUI.SetPurchased();
                levelManager.inventory.AddClothingKit(selectedKitUI.clothingKit);
                OnKitDeselect();
                //purchaseButton.interactable = false;
                completeStageButton.interactable = inventory.ownedKits.Count > 0;
            }
        }

        public void UpdateMoneyUI(int moneyAmount)
        {
            moneyUI.Set("$" + String.Format("{0:0.00}", moneyAmount));
        }
    }
}
