using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup.UI
{
    public class DressupStage : LevelStage
    {
        [SerializeField] private ModelUI modelUI;
        [SerializeField] private Transform itemAnchor;
        [SerializeField] private ItemUI itemPrefab;
        [SerializeField] private Button fitCheckButton;

        [Space]

        public ContestantData contestant;

        [Space]

        public FilterMode filterMode = FilterMode.OR;
        [SerializeField] private List<GarmentType> filterTypes = new List<GarmentType>();
        [SerializeField] private List<ClothingTag> filterTags = new List<ClothingTag>();
        public enum FilterMode {AND, OR}
        
        [Space]

        [SerializeField] private List<GarmentScriptable> clothes = new List<GarmentScriptable>();
        private List<ItemUI> items;

        private void Start()
        {
            items = new List<ItemUI>();
            
            foreach(GarmentScriptable c in clothes){
                ItemUI itemUI = Instantiate(itemPrefab.gameObject, itemAnchor).GetComponent<ItemUI>();
                itemUI.Set(c);
                itemUI.onSelect += OnItemSelected;
                items.Add(itemUI);
            }

            fitCheckButton.onClick.AddListener(OnFitCheck);
        }

        protected override void OnDestroy(){
            base.OnDestroy();
            fitCheckButton.onClick.RemoveListener(OnFitCheck);
        }

        private void OnItemSelected(GarmentScriptable garment){
            if (modelUI != null) {
                modelUI.SelectItem(garment);
            }
        }

        [Button]
        public void OnFitCheck(){
            modelUI.FitCheck();
        }
        
        [Button]
        public void Validate(){

        }
        
        [Button]
        public void UpdateFilter(){
            foreach(ItemUI item in items){
                
                if(item.garment == null ||
                (!filterTypes.IsNullOrEmpty() && !filterTypes.Contains(item.garment.type))){
                    item.Show(false);
                    continue;
                }

                List<ClothingTag> tags = item.Tags;
                bool enabled = filterTags.IsNullOrEmpty();

                foreach(ClothingTag tag in tags){
                    if(filterMode == FilterMode.OR && (enabled || tags.Contains(tag))){
                        enabled = true;
                        break;
                    }
                    else if(filterMode == FilterMode.AND && !tags.Contains(tag)){
                        enabled = false;
                        break;
                    }
                }

                item.Show(enabled);
            }
        }

        [Button]
        public void ClearFilter(){
            filterTypes.Clear();
            filterTags.Clear();
            UpdateFilter();
        }
    }
}
