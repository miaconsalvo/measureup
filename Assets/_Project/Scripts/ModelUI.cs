using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup.UI
{
    public class ModelUI : MonoBehaviour
    {
        public Transform modelAnchor;
        public Image clothingImage;
        private Dictionary<GarmentType, GarmentScriptable> garments;        
        private Dictionary<GarmentType, Image> garmentsUI;
        
        public void Awake(){
            clothingImage.gameObject.SetActive(false);
            garments = new Dictionary<GarmentType,GarmentScriptable>();
            garmentsUI = new Dictionary<GarmentType,Image>();
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

            Debug.Log("Set " + garment.type + ": " + garment.name);
        }

        public void RemoveItem(GarmentScriptable garment){
            if(garment == null) return;

            if (garments.ContainsKey(garment.type) && garments[garment.type] == garment){
                garments[garment.type] = null;
                if (garmentsUI.ContainsKey(garment.type)){
                    garmentsUI[garment.type].gameObject.SetActive(false);
                    garmentsUI[garment.type].sprite = null;
                }
            }

            Debug.Log("Remove " + garment.type);
        }
    }
}
