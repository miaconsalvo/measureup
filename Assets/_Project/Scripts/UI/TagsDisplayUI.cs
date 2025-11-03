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
        [SerializeField] private TagLabelUI tagUIPrefab;
        [SerializeField] private bool disableWhenEmpty = false;
        [SerializeField] private List<ClothingTag> tags = new List<ClothingTag>();
        [SerializeField] private List<ClothingTag.TagType> order = new List<ClothingTag.TagType>();
        [SerializeField] private List<ClothingTag.TagType> filter = new List<ClothingTag.TagType>();

        private List<TagLabelUI> tagsUI = new List<TagLabelUI>();
        private List<TagLabelUI> currentTagsUI = new List<TagLabelUI>();

        private void Awake()
        {
            for (int i = tagsAnchor.childCount - 1; i >= 0; i--)
            {
                Destroy(tagsAnchor.GetChild(i).gameObject);
            }

            if (!tags.IsNullOrEmpty()) SetTags(tags);
            else if (disableWhenEmpty) tagsAnchor.gameObject.SetActive(false);
        }

        public List<TagLabelUI> SetTags(List<ClothingTag> newTags)
        {
            ClearTags();
            tags.Clear();

            if (tagUIPrefab == null || newTags.IsNullOrEmpty()) return currentTagsUI;

            if (filter.Count > 0)
            {
                foreach (ClothingTag tag in newTags)
                {
                    if (filter.Contains(tag.type)) tags.Add(tag);
                }
            }
            else tags = newTags;

            if (tags.IsNullOrEmpty()) return currentTagsUI;

            tags = tags.OrderBy(tag => order.IndexOf(tag.type)).ToList();

            Canvas.ForceUpdateCanvases();

            GenerateTags(tags.Count);

            for (int i = 0; i < tags.Count; i++)
            {
                tagsUI[i].Set(tags[i]);
                tagsUI[i].gameObject.SetActive(true);
                currentTagsUI.Add(tagsUI[i]);
            }

            tagsAnchor.gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();

            return currentTagsUI;
        }

        private void GenerateTags(int amount)
        {
            if (tagUIPrefab == null) return;
            for (int i = tagsUI.Count; i < amount; i++)
            {
                //if (i > tagsUI.Count) continue;
                TagLabelUI tagUI = Instantiate(tagUIPrefab.gameObject, tagsAnchor).GetComponent<TagLabelUI>();
                tagsUI.Add(tagUI);
            }

            ClearTags();
        }

        public void ClearTags()
        {
            currentTagsUI = new List<TagLabelUI>();
            foreach (TagLabelUI tagUI in tagsUI) tagUI.Deactivate();
            if (disableWhenEmpty) tagsAnchor.gameObject.SetActive(false);
        }
    }
}
