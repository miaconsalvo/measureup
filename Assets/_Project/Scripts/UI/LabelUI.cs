using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Mystie
{
    public class LabelUI : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent labelLocalized;
        [SerializeField] private TextMeshProUGUI label;

        public void Set(LocalizedString textLocalized)
        {
            labelLocalized.StringReference = textLocalized;
            Set(textLocalized.GetLocalizedString());
        }

        public void Set(string text)
        {
            label.text = text;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            label = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
