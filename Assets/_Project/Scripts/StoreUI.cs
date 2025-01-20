using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mystie
{
    public class StoreUI : MonoBehaviour
    {
        [SerializeField] private ClothingKitUI detailsPanel;
        [SerializeField] private Transform kitPreviewsAnchor;
        [SerializeField] private ClothingKitUI kitPreviewPrefab;
        
        private List<ClothingKitUI> kitUIs;

        [Space]

        [SerializeField] private LabelUI moneyUI;
        [SerializeField] private int money;

        [Space]

        [SerializeField] private List<ClothingKitScriptable> kits = new List<ClothingKitScriptable>();

        private void Start()
        {
            kitUIs = new List<ClothingKitUI>();
            foreach(Transform child in kitPreviewsAnchor){
                GameObject.Destroy(child.gameObject);
            }
            
            foreach(ClothingKitScriptable kit in kits){
                ClothingKitUI clothingKitUI = Instantiate(kitPreviewPrefab.gameObject, kitPreviewsAnchor).GetComponent<ClothingKitUI>();
                clothingKitUI.Set(kit);
                clothingKitUI.onSelect += OnKitSelected;
                kitUIs.Add(clothingKitUI);
            }

            detailsPanel.gameObject.SetActive(false);
            moneyUI.Set("$" + String.Format("{0:0.00}", money));
        }

        private void OnKitSelected(ClothingKitScriptable kitUI){
            if (detailsPanel != null) {
                detailsPanel.gameObject.SetActive(true);
                detailsPanel.Set(kitUI);
            }
        }

        private void OnKitDeselect(){
            if (detailsPanel != null) {
                detailsPanel.gameObject.SetActive(false);
            }
        }

        private void SpendMoney(int amount){
            money -= amount;
            money = Math.Max(money, 0);
        }
    }
}
