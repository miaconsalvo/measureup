using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public enum FilterMode { AND, OR }

        [Space]

        private List<GarmentScriptable> clothes;
        private List<ItemUI> items;

        private void Start()
        {
            UpdateItems();

            fitCheckButton.onClick.AddListener(OnFitCheck);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (ItemUI item in items.ToList())
            {
                DestroyItemUI(item);
            }
            fitCheckButton.onClick.RemoveListener(OnFitCheck);
        }

        protected override void OnStageEnter()
        {
            base.OnStageEnter();
            UpdateItems();
        }

        public void UpdateItems()
        {
            if (items.IsNullOrEmpty()) items = new List<ItemUI>();
            clothes = InventoryManager.Instance.clothes;

            for (int i = 0; i < clothes.Count; i++)
            {
                if (i < items.Count) items[i].Set(null);
                else CreateItemUI(clothes[i]);
            }

            for (int i = clothes.Count; i < items.Count; i++)
            {
                items[i].Set(null);
                items[i].gameObject.SetActive(false);
            }
        }

        private ItemUI CreateItemUI(GarmentScriptable c)
        {
            ItemUI itemUI = Instantiate(itemPrefab.gameObject, itemAnchor).GetComponent<ItemUI>();
            itemUI.Set(c);
            itemUI.onSelect += OnItemSelected;
            items.Add(itemUI);
            return itemUI;
        }

        private void DestroyItemUI(ItemUI itemUI)
        {
            itemUI.onSelect -= OnItemSelected;
            items.Remove(itemUI);
            Destroy(itemUI.gameObject);
        }

        private void OnItemSelected(GarmentScriptable garment)
        {
            if (modelUI != null)
            {
                modelUI.SelectItem(garment);
            }
        }

        [Button]
        public void OnFitCheck()
        {
            modelUI.FitCheck();
        }

        [Button]
        public void Validate()
        {

        }

        [Button]
        public void UpdateFilter()
        {
            foreach (ItemUI item in items)
            {

                if (item.garment == null ||
                (!filterTypes.IsNullOrEmpty() && !filterTypes.Contains(item.garment.type)))
                {
                    item.Show(false);
                    continue;
                }

                List<ClothingTag> tags = item.Tags;
                bool enabled = filterTags.IsNullOrEmpty();

                foreach (ClothingTag tag in tags)
                {
                    if (filterMode == FilterMode.OR && (enabled || tags.Contains(tag)))
                    {
                        enabled = true;
                        break;
                    }
                    else if (filterMode == FilterMode.AND && !tags.Contains(tag))
                    {
                        enabled = false;
                        break;
                    }
                }

                item.Show(enabled);
            }
        }

        [Button]
        public void ClearFilter()
        {
            filterTypes.Clear();
            filterTags.Clear();
            UpdateFilter();
        }
    }
}
