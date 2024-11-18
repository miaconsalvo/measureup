using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class SliderAnimator : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Animator anim;
        [SerializeField] string selectAnim = "Select";

        private void Start()
        {
            if (slider == null) 
                slider = GetComponentInChildren<Slider>();

            slider.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(slider.value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (anim != null)
            {
                anim.Play(selectAnim);
            }
        }

        void OnValueChanged(float value)
        {
            if (label != null)
            {
                label.text = (float)Math.Round((double)slider.value, 1) + "";
            }
        }

        void Reset()
        {
            slider = GetComponentInChildren<Slider>();
        }
    }
}
