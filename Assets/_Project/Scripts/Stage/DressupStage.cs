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
        [SerializeField] private DressupModel model;
        [SerializeField] private Transform itemAnchor;
        [SerializeField] private ItemUI itemPrefab;
        [SerializeField] private Button fitCheckButton;
        [SerializeField] private ItemDetailsPanelUI itemDetailsPanelUI;
        [SerializeField] private LabelUI opinionBox;
        [SerializeField] private float opinionDuration = 4f;

        [Space]

        public ContestantData contestant;

        [Space]

        public FilterMode filterMode = FilterMode.OR;
        [SerializeField] private List<GarmentType> filterTypes = new List<GarmentType>();
        [SerializeField] private List<ClothingTag> filterTags = new List<ClothingTag>();
        public enum FilterMode { AND, OR }

        [Space]

        private List<ItemScriptable> clothes;
        private List<ItemUI> items;

        private void Start()
        {
            if (itemDetailsPanelUI != null) itemDetailsPanelUI.Hide(false);
            UpdateItems();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            fitCheckButton.onClick.AddListener(OnFitCheck);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            fitCheckButton.onClick.RemoveListener(OnFitCheck);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (ItemUI item in items.ToList())
            {
                DestroyItemUI(item);
            }

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
                if (i < items.Count) items[i].Set(clothes[i]);
                else CreateItemUI(clothes[i]);
            }

            for (int i = clothes.Count; i < items.Count; i++)
            {
                items[i].Set(null);
                items[i].gameObject.SetActive(false);
            }
        }

        private ItemUI CreateItemUI(ItemScriptable c)
        {
            ItemUI itemUI = Instantiate(itemPrefab.gameObject, itemAnchor).GetComponent<ItemUI>();
            itemUI.Init(c, OnItemSelected, model.AddItem, model.RemoveItem);
            items.Add(itemUI);
            return itemUI;
        }

        private void DestroyItemUI(ItemUI itemUI)
        {
            items.Remove(itemUI);
            Destroy(itemUI.gameObject);
        }

        private void OnItemSelected(ItemScriptable item)
        {
            StartCoroutine(OnItemSelectedCoroutine(item));
        }

        public IEnumerator OnItemSelectedCoroutine(ItemScriptable item)
        {
            if (itemDetailsPanelUI == null || itemDetailsPanelUI.item == item) yield break;

            if (itemDetailsPanelUI.item != null)
            {
                itemDetailsPanelUI.Hide();
                yield return new WaitForSeconds(itemDetailsPanelUI.fadeTime);
            }

            if (itemDetailsPanelUI.item != item)
            {
                itemDetailsPanelUI.Set(item);
                if (item != null) itemDetailsPanelUI.Show();
            }
            else
            {
                itemDetailsPanelUI.Set(null);
            }
        }

        [Button]
        public void OnFitCheck()
        {
            string opinion = model.FitCheck();
            if (opinionBox != null) StartCoroutine(ShowOpinionCoroutine(opinion));
        }

        public IEnumerator ShowOpinionCoroutine(string opinion)
        {
            if (opinionBox.isVisible)
            {
                opinionBox.StopAllCoroutines();
                opinionBox.Hide();
                yield return new WaitForSeconds(opinionBox.fadeTime);
            }

            opinionBox.Set(opinion);
            opinionBox.ShowForDuration(opinionDuration);
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

                if (item.item == null ||
                (!filterTypes.IsNullOrEmpty() && !filterTypes.Contains(item.item.type)))
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
