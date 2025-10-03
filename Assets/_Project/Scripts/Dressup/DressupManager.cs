using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie.Dressup
{
    public enum Reaction { Neutral = 0, Positive = 1, Negative = 2 }

    public class DressupManager : MonoBehaviour
    {
        public event Action<ItemScriptable> onItemAdded;
        public event Action<ItemScriptable> onItemRemoved;

        public ContestantData contestant { get; private set; }
        public Reaction reaction { get; private set; }
        [SerializeField] private Dictionary<ItemType, ItemScriptable> items;

        public List<ClothingTag> currentTags;

        private List<LocalizedString> opinionsAvailable;
        private List<LocalizedString> opinionsUsed;

        private int negativeCount = 0;
        private int positiveCount = 0;

        [SerializeField] private Dictionary<ItemType, ItemScriptable> startingItems;
        [SerializeField] private Dictionary<ItemType, ItemScriptable> underwearItems;

        public void Initialize(ContestantData contestantData)
        {
            items = new Dictionary<ItemType, ItemScriptable>();

            currentTags = new List<ClothingTag>();
            opinionsAvailable = new List<LocalizedString>();
            opinionsUsed = new List<LocalizedString>();

            contestant = contestantData;
            SetModel(contestant);
        }

        public void SetModel(ContestantData newContestant)
        {
            contestant = newContestant;

            if (contestant == null) return;

            startingItems = new Dictionary<ItemType, ItemScriptable>();
            underwearItems = new Dictionary<ItemType, ItemScriptable>();

            foreach (ItemScriptable item in contestant.startingClothes) startingItems.Add(item.type, item);
            foreach (ItemScriptable item in contestant.underwear) underwearItems.Add(item.type, item);

            foreach (ItemScriptable item in startingItems.Values) AddItem(item);
            foreach (ItemType itemType in underwearItems.Keys)
            {
                if (underwearItems.ContainsKey(itemType) && !items.ContainsKey(itemType))
                    AddItem(underwearItems[itemType]);
            }

        }

        public string FitCheck()
        {
            opinionsAvailable.Clear();

            foreach (OpinionCondition condition in contestant.opinionConditions)
            {
                if (MeetsConditions(condition))
                {
                    opinionsAvailable.AddRange(condition.opinions);
                }
            }

            opinionsAvailable = opinionsAvailable.Except(opinionsUsed).ToList();

            if (opinionsAvailable.Count > 0)
            {
                int rand = UnityEngine.Random.Range(0, opinionsAvailable.Count);
                LocalizedString opinion = opinionsAvailable[rand];
                //opinionsAvailable.RemoveAt(rand)
                opinionsUsed.Add(opinion);
                return opinion.GetLocalizedString(new { items = items.Values.ToList() });
            }
            else
            {
                return contestant.opinionDefault.GetLocalizedString();
            }
        }

        public bool IsFitAppropriate()
        {
            foreach (ItemType type in items.Keys)
            {
                if (items[type] == underwearItems[type]) return false;
            }

            return true;
        }

        public void UpdateTags()
        {
            // Check positive and negative tags count
            negativeCount = 0;
            positiveCount = 0;

            foreach (ClothingTag tag in currentTags)
            {
                if (contestant.positiveTags.Contains(tag))
                    positiveCount++;
                if (contestant.negativeTags.Contains(tag))
                    negativeCount++;
            }
        }

        public void AddItem(ItemScriptable item)
        {
            if (item == null || items.ContainsValue(item))
                return;

            if (items.ContainsKey(item.type))
            {
                if (items[item.type] != null && items[item.type] != item)
                    RemoveItem(items[item.type], true);
                items[item.type] = item;
            }
            else items.Add(item.type, item);



            foreach (ClothingTag tag in item.tags)
            {
                currentTags.Add(tag);
            }

            onItemAdded?.Invoke(item);

            UpdateTags();

            Debug.Log("Added " + item.name + "(" + item.type + ")");
        }

        public void RemoveItem(ItemScriptable item, bool swapping = false)
        {
            if (item == null || !items.ContainsValue(item)) return;

            foreach (ClothingTag tag in item.tags)
            {
                if (currentTags.Contains(tag)) currentTags.Remove(tag);
            }
            items[item.type] = null;

            onItemRemoved?.Invoke(item);

            if (!swapping && !contestant.underwear.Contains(item) && underwearItems.ContainsKey(item.type))
                AddItem(underwearItems[item.type]);

            UpdateTags();

            Debug.Log("Removed " + item.name + "(" + item.type + ")");
        }

        private bool MeetsConditions(OpinionCondition condition)
        {
            // Check required tags
            foreach (ClothingTag tag in condition.requiredTags)
            {
                if (!currentTags.Contains(tag)) return false;
            }

            // Check excluded tags
            foreach (ClothingTag tag in condition.excludedTags)
            {
                if (currentTags.Contains(tag)) return false;
            }

            if (negativeCount < condition.minNegativeTags
            || positiveCount < condition.minPositiveTags)
            {
                return false;
            }

            return true;
        }

        public bool CheckStyleRule()
        {
            return LevelManager.Instance.episode.styleRule.Check(currentTags);
        }

        public bool CheckTrending()
        {
            return LevelManager.Instance.episode.trendingRule.Check(currentTags);
        }

        // TODO Implement this better
        [YarnFunction("has_tag")]
        public static bool HasTag(string s)
        {
            //Debug.Log("Has tag " + s);
            foreach (ClothingTag tag in LevelManager.Instance.dressup.currentTags)
            {
                if (tag.name == s) return true;
            }
            return false;
        }

        [YarnFunction("neg_tags")]
        public static int GetNegativeTags()
        {
            return LevelManager.Instance.dressup.negativeCount;
        }

        [YarnFunction("pos_tags")]
        public static int GetPositiveTag()
        {
            return LevelManager.Instance.dressup.positiveCount;
        }

        [YarnFunction("get_item")]
        public static string GetItemWithTag(string s)
        {
            if (!HasTag(s)) return null;

            foreach (ItemScriptable item in LevelManager.Instance.dressup.items.Values)
            {
                if (item.HasTag(s)) return item.displayName.GetLocalizedString().ToLower();
            }

            return null;
        }

        [YarnCommand("set_reaction")]
        public static void SetReaction(int reaction)
        {
            LevelManager.Instance.dressup.reaction = (Reaction)reaction;
        }

        [YarnFunction("get_reaction")]
        public static int GetReaction()
        {
            return (int)LevelManager.Instance.dressup.reaction;
        }
    }
}
