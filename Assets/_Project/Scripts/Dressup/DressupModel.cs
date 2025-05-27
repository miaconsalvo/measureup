using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Febucci.UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Mystie.Dressup
{
    public class DressupModel : MonoBehaviour
    {
        private static DressupModel instance;

        public ContestantData contestant;
        public Transform modelAnchor;
        public Image clothingImage;
        [SerializeField] private Dictionary<GarmentType, ItemScriptable> items;
        private Dictionary<GarmentType, Image> itemsUI;
        public List<ClothingTag> currentTags;

        public List<string> opinionsAvailable;
        public List<string> opinionsUsed;

        private int negativeCount = 0;
        private int positiveCount = 0;

        [SerializeField] private List<ItemScriptable> startingItems;

        public void Awake()
        {
            instance = this;

            clothingImage.gameObject.SetActive(false);
            items = new Dictionary<GarmentType, ItemScriptable>();
            itemsUI = new Dictionary<GarmentType, Image>();
            currentTags = new List<ClothingTag>();
            opinionsAvailable = new List<string>();
            opinionsUsed = new List<string>();

            foreach (ItemScriptable item in startingItems) AddItem(item);
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
                int rand = Random.Range(0, opinionsAvailable.Count);
                string opinion = opinionsAvailable[rand];
                opinionsUsed.Add(opinion);
                return opinion;
                //opinionsAvailable.RemoveAt(rand);
            }
            else
            {
                return "Hmmm… yeah this is okay.";
            }
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
            if (item == null || items.ContainsValue(item)) return;

            if (!items.ContainsKey(item.type))
            {
                items.Add(item.type, item);
            }

            if (!itemsUI.ContainsKey(item.type))
            {
                Image img = Instantiate(clothingImage.gameObject, modelAnchor).GetComponent<Image>();
                itemsUI.Add(item.type, img);
            }

            items[item.type] = item;
            itemsUI[item.type].gameObject.SetActive(item.sprite != null);
            itemsUI[item.type].sprite = item.sprite;
            itemsUI[item.type].SetNativeSize();

            foreach (ClothingTag tag in item.tags)
            {
                currentTags.Add(tag);
            }

            UpdateTags();

            Debug.Log("Added " + item.type + "(" + item.type + ")");
        }

        public void RemoveItem(ItemScriptable item)
        {
            if (item == null || !items.ContainsValue(item)) return;

            foreach (ClothingTag tag in item.tags)
            {
                if (currentTags.Contains(tag)) currentTags.Remove(tag);
            }
            items[item.type] = null;
            if (itemsUI.ContainsKey(item.type))
            {
                itemsUI[item.type].gameObject.SetActive(false);
                itemsUI[item.type].sprite = null;
            }

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

        // TODO Implement this better
        [YarnFunction("has_tag")]
        public static bool HasTag(string s)
        {
            //Debug.Log("Has tag " + s);
            foreach (ClothingTag tag in instance.currentTags)
            {
                if (tag.name == s) return true;
            }
            return false;
        }

        [YarnFunction("neg_tags")]
        public static int GetNegativeTags()
        {
            return instance.negativeCount;
        }

        [YarnFunction("pos_tags")]
        public static int GetPositiveTag()
        {
            return instance.positiveCount;
        }

        [YarnFunction("get_item")]
        public static string GetItemWithTag(string s)
        {
            if (!HasTag(s)) return null;

            foreach (ItemScriptable item in instance.items.Values)
            {
                if (item.HasTag(s)) return item.displayName.GetLocalizedString();
            }

            return null;
        }
    }
}
