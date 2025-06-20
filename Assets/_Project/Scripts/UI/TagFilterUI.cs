using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class TagFilterUI : MonoBehaviour
    {
        [SerializeField] private DressupStage dressup;
        [SerializeField] private CanvasGroup panel;
        [SerializeField] private RectTransform panelRect;

        [SerializeField] private Button filterButton;
        [SerializeField] private Button closeButton;

        private List<ClothingTag> tagsInInventory;

        [field: SerializeField] public TypeFilter typeFilter { get; private set; }
        [SerializeField] private List<Filter> filters;

        [SerializeField] private Dictionary<ItemUI, bool> tagDict;

        public void Awake()
        {
            dressup.onItemListUpdate += UpdateTagsList;
            dressup.onItemUIListUpdate += OnItemUIListUpdate;
            filterButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);

            typeFilter.onUpdate += UpdateFilter;
            foreach (Filter f in filters) f.filter.onUpdate += UpdateFilter;

            Close();
        }

        public void OnDestroy()
        {
            dressup.onItemListUpdate -= UpdateTagsList;
            dressup.onItemUIListUpdate -= OnItemUIListUpdate;
            filterButton.onClick.RemoveListener(Open);
            closeButton.onClick.RemoveListener(Close);

            typeFilter.onUpdate -= UpdateFilter;
            foreach (Filter f in filters) f.filter.onUpdate -= UpdateFilter;
        }

        public void OnItemUIListUpdate(List<ItemUI> newItemsUI)
        {
            tagDict = new Dictionary<ItemUI, bool>();
            foreach (ItemUI ui in newItemsUI)
            {
                tagDict.Add(ui, true);
            }

            UpdateFilter();
        }

        [Button]
        public void UpdateFilter()
        {
            foreach (ItemUI ui in tagDict.Keys.ToList()) tagDict[ui] = true;

            typeFilter.ApplyFilter(ref tagDict);

            foreach (Filter f in filters) f.filter.ApplyFilter(ref tagDict);

            foreach (ItemUI ui in tagDict.Keys) ui.Show(tagDict[ui]);
        }

        public void UpdateTagsList(List<ItemScriptable> items)
        {
            tagsInInventory = new List<ClothingTag>();
            foreach (ItemScriptable item in items)
            {
                foreach (ClothingTag tag in item.tags)
                {
                    if (!tagsInInventory.Contains(tag))
                    {
                        tagsInInventory.Add(tag);
                        //if (!tagDict.ContainsKey(tag.type))
                        //tagDict.Add(tag.type, new List<ClothingTag>());
                        //tagDict[tag.type].Add(tag);
                    }
                }
            }

            UpdateFilter(tagsInInventory);
        }

        public void UpdateFilter(List<ClothingTag> tags)
        {
            foreach (Filter f in filters)
            {
                List<TagLabelUI> tagsUI = f.display.SetTags(tagsInInventory);
                if (tagsUI.IsNullOrEmpty()) continue;
                foreach (TagLabelUI tagUI in tagsUI)
                {
                    if (tagUI.toggle != null) tagUI.toggle.onValueChanged.AddListener((value) => f.filter.Set(tagUI.tag, value));
                }
            }

            UpdateUI();
        }

        [Button]
        public void ClearFilter()
        {
            typeFilter.Clear();
            foreach (Filter f in filters) f.filter.Clear();
            UpdateFilter();
        }

        private void Close()
        {
            //panel.gameObject.SetActive(false);
            panel.alpha = 0f;
            panel.blocksRaycasts = false;
            filterButton.gameObject.SetActive(true);

        }

        private void Open()
        {
            //panel.gameObject.SetActive(true);
            panel.alpha = 1.0f;
            panel.blocksRaycasts = true;
            filterButton.gameObject.SetActive(false);
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (panelRect != null) LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
        }

        [Serializable]
        public class Filter
        {
            [field: SerializeField] public TagsDisplayUI display { get; private set; }
            [field: SerializeField] public TagFilter filter { get; private set; }


        }
    }
}
