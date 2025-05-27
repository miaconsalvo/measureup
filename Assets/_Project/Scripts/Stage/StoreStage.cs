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
        [SerializeField] private ClothingKitUI detailsPanel;
        [SerializeField] private Transform kitPreviewsAnchor;
        [SerializeField] private ClothingKitUI kitPreviewPrefab;
        private ClothingKitUI selectedKitUI;

        private List<ClothingKitUI> kitUIs;

        [Space]

        [SerializeField] private Button purchaseButton;
        [SerializeField] private LabelUI moneyUI;
        [SerializeField] private int money;

        [Space]

        [SerializeField] private LocalizedString purchaseConfirmText;

        [Space]

        [SerializeField] private List<ClothingKitScriptable> kits = new List<ClothingKitScriptable>();

        protected override void Awake()
        {
            base.Awake();
            purchaseButton.onClick.AddListener(OnPurchase);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            purchaseButton.onClick.RemoveListener(OnPurchase);
        }

        private void Start()
        {
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

            detailsPanel.gameObject.SetActive(false);
            UpdateMoneyUI();
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
            money += amount;
            UpdateMoneyUI();
            Debug.Log(amount + "$ gained. New total is " + money + "$.");
        }

        public bool CanBuy(int price)
        {
            return price <= money;
        }

        public bool Purchase(int price)
        {
            if (!CanBuy(price)) return false;

            money = Math.Max(money - price, 0);
            Debug.Log(price + "$ spent. New total is " + money + "$.");

            UpdateMoneyUI();

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
                InventoryManager.Instance.AddClothes(selectedKitUI.clothingKit);
                OnKitDeselect();
                //purchaseButton.interactable = false;
            }
        }

        public void UpdateMoneyUI()
        {
            moneyUI.Set("$" + String.Format("{0:0.00}", money));
        }
    }
}
