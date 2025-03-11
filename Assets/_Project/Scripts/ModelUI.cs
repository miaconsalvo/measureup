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
        private Dictionary<GarmentType, GarmentScriptable> garments;        
        private Dictionary<GarmentType, Image> garmentsUI;
        public List<ClothingTag> currentTags;

        public List<string> opinionsAvailable;
        public List<string> opinionsUsed;
        
        public void Awake(){
            clothingImage.gameObject.SetActive(false);
            garments = new Dictionary<GarmentType,GarmentScriptable>();
            garmentsUI = new Dictionary<GarmentType,Image>();
            currentTags = new List<ClothingTag>();
            opinionsAvailable = new List<string>();
            opinionsUsed = new List<string>();
        }

        public void FitCheck()
        {
            opinionsAvailable.Clear();

            foreach(OpinionCondition condition in contestant.opinionConditions){
                if(MeetsConditions(condition)){
                    opinionsAvailable.AddRange(condition.opinions);
                }
            }

            opinionsAvailable = opinionsAvailable.Except(opinionsUsed).ToList();

            if(opinionsAvailable.Count > 0){
                int rand = Random.Range(0, opinionsAvailable.Count);
                string opinion = opinionsAvailable[rand];
                opinionsUsed.Add(opinion);
                Debug.Log(opinion);
                //opinionsAvailable.RemoveAt(rand);
            }
            else{
                Debug.Log("Hmmm… yeah this is okay.");
            }
        }

        public void SelectItem(GarmentScriptable garment){
            if (garment == null) return;
            if (garments.ContainsKey(garment.type) && garments[garment.type] == garment){
                RemoveItem(garment);
            }
            else SetItem(garment);
        }

        public void SetItem(GarmentScriptable garment){
            if (garment == null) return;

            if (!garments.ContainsKey(garment.type)){
                garments.Add(garment.type, garment);
            }

            if (!garmentsUI.ContainsKey(garment.type)){
                Image img = Instantiate(clothingImage.gameObject, modelAnchor).GetComponent<Image>();
                garmentsUI.Add(garment.type, img);
            }

            garments[garment.type] = garment;
            garmentsUI[garment.type].gameObject.SetActive(garment.sprite != null);
            garmentsUI[garment.type].sprite = garment.sprite;
            garmentsUI[garment.type].SetNativeSize();

            foreach(ClothingTag tag in garment.tags) {
                currentTags.Add(tag);
            }

            Debug.Log("Set " + garment.type + ": " + garment.name);
        }

        public void RemoveItem(GarmentScriptable garment){
            if(garment == null) return;

            if (garments.ContainsKey(garment.type) && garments[garment.type] == garment){
                foreach(ClothingTag tag in garment.tags) {
                    if(currentTags.Contains(tag)) currentTags.Remove(tag);
                }
                garments[garment.type] = null;
                if (garmentsUI.ContainsKey(garment.type)){
                    garmentsUI[garment.type].gameObject.SetActive(false);
                    garmentsUI[garment.type].sprite = null;
                }
            }

            Debug.Log("Remove " + garment.type);
        }
    
        private bool MeetsConditions(OpinionCondition condition){
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
