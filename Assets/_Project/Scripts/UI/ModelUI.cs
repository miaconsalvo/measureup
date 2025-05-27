using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Febucci.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup.UI
{
    public class ModelUI : MonoBehaviour
    {
        public ContestantData contestant;
        public Transform modelAnchor;
        public Image clothingImage;
        private Dictionary<GarmentType, ItemScriptable> items;
        private Dictionary<GarmentType, Image> itemsUI;
        public List<ClothingTag> currentTags;

        public List<string> opinionsAvailable;
        public List<string> opinionsUsed;

        public void Awake()
        {
            clothingImage.gameObject.SetActive(false);
            items = new Dictionary<GarmentType, ItemScriptable>();
            itemsUI = new Dictionary<GarmentType, Image>();
            currentTags = new List<ClothingTag>();
            opinionsAvailable = new List<string>();
            opinionsUsed = new List<string>();
        }

        public void FitCheck()
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
                Debug.Log(opinion);
                //opinionsAvailable.RemoveAt(rand);
            }
            else
            {
                Debug.Log("Hmmm… yeah this is okay.");
            }
        }

        public void SelectItem(ItemScriptable garment)
        {
            if (garment == null) return;
            if (items.ContainsKey(garment.type) && items[garment.type] == garment)
            {
                RemoveItem(garment);
            }
            else SetItem(garment);
        }

        public void SetItem(ItemScriptable item)
        {
            if (item == null) return;

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

            Debug.Log("Set " + item.type + ": " + item.name);
        }

        public void RemoveItem(ItemScriptable garment)
        {
            if (garment == null) return;

            if (items.ContainsKey(garment.type) && items[garment.type] == garment)
            {
                foreach (ClothingTag tag in garment.tags)
                {
                    if (currentTags.Contains(tag)) currentTags.Remove(tag);
                }
                items[garment.type] = null;
                if (itemsUI.ContainsKey(garment.type))
                {
                    itemsUI[garment.type].gameObject.SetActive(false);
                    itemsUI[garment.type].sprite = null;
                }
            }

            Debug.Log("Remove " + garment.type);
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

            // Check positive and negative tags count
            int negativeCount = 0;
            int positiveCount = 0;
            foreach (ClothingTag tag in currentTags)
            {
                if (contestant.positiveTags.Contains(tag))
                    positiveCount++;
                if (contestant.negativeTags.Contains(tag))
                    negativeCount++;
            }

            if (negativeCount < condition.minNegativeTags
            || positiveCount < condition.minPositiveTags)
            {
                return false;
            }

            return true;
        }
    }
}
