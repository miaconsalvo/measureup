using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mystie
{
    public class LabelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void Set(string text){
            label.text = text;
            gameObject.SetActive(true);
        }

        public void Deactivate(){
            gameObject.SetActive(false);
        }

        public void Reset(){
            label = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
