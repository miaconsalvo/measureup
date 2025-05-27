using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using Mystie.Dressup;
using UnityEngine;
using UnityEngine.Localization;

namespace Mystie
{
    public class TagsDisplayUI : MonoBehaviour
    {
        [SerializeField] private Transform tagsAnchor;
        [SerializeField] private LabelUI tagUIPrefab;
        [SerializeField] private bool disableWhenEmpty = false;
        [SerializeField] private List<ClothingTag> tags = new List<ClothingTag>();
        [SerializeField] private List<ClothingTag.TagType> order = new List<ClothingTag.TagType>();
        [SerializeField] private List<ClothingTag.TagType> filter = new List<ClothingTag.TagType>();

        private List<LabelUI> tagsUI = new List<LabelUI>();

        private void Awake()
        {
            for (int i = tagsAnchor.childCount - 1; i >= 0; i--)
            {
                Destroy(tagsAnchor.GetChild(i).gameObject);
            }

            if (!tags.IsNullOrEmpty()) SetTags(tags);
            else if (disableWhenEmpty) tagsAnchor.gameObject.SetActive(false);
        }

        public void SetTags(List<ClothingTag> newTags)
        {
            ClearTags();
            tags.Clear();

            if (tagUIPrefab == null || newTags.IsNullOrEmpty()) return;

            if (filter.Count > 0)
            {
                foreach (ClothingTag tag in newTags)
                {
                    if (filter.Contains(tag.type)) tags.Add(tag);
                }
            }
            else tags = newTags;

            if (tags.IsNullOrEmpty()) return;

            tags = tags.OrderBy(tag => order.IndexOf(tag.type)).ToList();

            Canvas.ForceUpdateCanvases();

            GenerateTags(tags.Count);

            int i = 0;
            foreach (ClothingTag tag in tags)
            {
                tagsUI[i].Set(tag.displayName, tag.color, tag.sprite);
                i++;
            }

            tagsAnchor.gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();
        }

        public void GenerateTags(int amount)
        {
            if (tagUIPrefab == null) return;
            for (int i = tagsUI.Count; i < amount; i++)
            {
                //if (i > tagsUI.Count) continue;
                LabelUI tagUI = Instantiate(tagUIPrefab.gameObject, tagsAnchor).GetComponent<LabelUI>();
                tagsUI.Add(tagUI);
            }

            ClearTags();
        }

        public void ClearTags()
        {
            foreach (LabelUI tagUI in tagsUI) tagUI.Deactivate();
            if (disableWhenEmpty) tagsAnchor.gameObject.SetActive(false);
        }
    }
}
