using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Dressup.UI
{
    public class DressupUI : MonoBehaviour
    {
        [SerializeField] private ModelUI modelUI;
        [SerializeField] private Transform itemAnchor;
        [SerializeField] private ItemUI itemPrefab;
        
        [Space]

        [SerializeField] private List<GarmentScriptable> clothes = new List<GarmentScriptable>();
        private List<ItemUI> items;

        private void Start()
        {
            items = new List<ItemUI>();
            
            foreach(GarmentScriptable c in clothes){
                ItemUI itemUI = Instantiate(itemPrefab.gameObject, itemAnchor).GetComponent<ItemUI>();
                itemUI.Set(c);
                itemUI.onSelect += OnItemSelected;
                items.Add(itemUI);
            }
        }

        private void OnItemSelected(GarmentScriptable garment){
            if (modelUI != null) {
                modelUI.SelectItem(garment);
            }
        }
    }
}
