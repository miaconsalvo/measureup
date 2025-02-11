using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class ItemUI : MonoBehaviour
    {
        public event Action<GarmentScriptable> onSelect;

        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private GarmentScriptable garment;
        
        private bool init = false;

        private void Awake(){
            if (button != null) button.onClick.AddListener(OnSelect);
        }

        private void Start(){
            if (!init) Set(garment);
        }

        public void Set(Sprite sprite){
            image.sprite = sprite;
            image.enabled = true;
        }

        public void SetEmpty(){
            image.sprite = null;
            image.enabled = false;
        }

        public void Set(GarmentScriptable garment){
            this.garment = garment;
            if (image != null && garment != null) 
                Set(garment.icon);
            else SetEmpty();
            init = true;
        }

        public void OnSelect(){
            onSelect?.Invoke(garment);
        }

        public void Reset(){
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        public void OnValidate(){
            Set(garment);
        }
    }
}
