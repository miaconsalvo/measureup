using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ClothingKitUI : MonoBehaviour
    {
        public event Action<ClothingKitScriptable> onSelect;

        public Button button{get; private set;}
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI priceRangeLabel;

        [Space]

        [SerializeField] private Transform itemsAnchor;
        private List<ItemUI> itemsUI = new List<ItemUI>();

        [Space]
        [SerializeField] private Transform tagsAnchor;  
        [SerializeField] private LabelUI tagUIPrefab;
        private List<LabelUI> tagsUI;

        [field: SerializeField] public ClothingKitScriptable clothingKit{get; private set;}

        private void Awake()
        {
            button = GetComponent<Button>();
            if (button != null) button.onClick.AddListener(OnSelect);
            
            tagsUI = new List<LabelUI>();
                
            itemsUI = itemsAnchor.GetComponentsInChildren<ItemUI>().ToList();
            Set(clothingKit);
        }

        private void OnDestroy() {
            if (button != null) button.onClick.RemoveAllListeners();
        }

        public void Set(ClothingKitScriptable kit){
            
            clothingKit = kit;
            
            if (kit != null){
                if (nameLabel != null) nameLabel.text = kit.name;
                if (priceLabel != null) priceLabel.text = "$" + String.Format("{0:0.00}", kit.price);
                if (priceRangeLabel != null) priceRangeLabel.text = kit.priceRange;
                
                if (tagUIPrefab != null) SetTags(kit.Tags);
            }
            else{
                if (nameLabel != null) nameLabel.text = "None";
                if (priceLabel != null) priceLabel.text = "$0.00";
                if (priceRangeLabel != null) priceRangeLabel.text = "";
                foreach(LabelUI tag in tagsUI) tag.gameObject.SetActive(false);
            }
            
            for(int i = 0; i < itemsUI.Count; i++){
                if(clothingKit != null && i < clothingKit.garments.Count) 
                    itemsUI[i].Set(clothingKit.garments[i]);
                else itemsUI[i].SetEmpty();
            }
        }

        public void SetTags(List<string> tags){
            if (tagUIPrefab == null) return;
            Canvas.ForceUpdateCanvases();
            GenerateTags(tags.Count);
            for(int i = 0; i < tags.Count; i++){
                tagsUI[i].Set(tags[i]);
            }
            Canvas.ForceUpdateCanvases();
        }

        public void GenerateTags(int amount){
            if (tagUIPrefab == null) return;
            for(int i = tagsUI.Count; i < amount; i++){
                if(i > tagsUI.Count) continue; 
                LabelUI tagUI = Instantiate(tagUIPrefab.gameObject, tagsAnchor).GetComponent<LabelUI>();
                tagUI.Deactivate();
                tagsUI.Add(tagUI);
            }
            foreach(LabelUI tagUI in tagsUI) tagUI.gameObject.SetActive(false);
        }

        public void OnSelect(){
            onSelect?.Invoke(clothingKit);
        }

        public void UpdateItems(){
            itemsUI = itemsAnchor.GetComponentsInChildren<ItemUI>().ToList();
        }
    }
}
