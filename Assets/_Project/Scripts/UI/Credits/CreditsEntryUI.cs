using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Mystie.UI
{
    public class CreditsEntryUI : MonoBehaviour
    {
        public TextMeshProUGUI nameLabel;
        public LocalizeStringEvent localizer;

        public void Set(string name, LocalizedString localizedString)
        {
            nameLabel.text = name;
            localizer.StringReference = localizedString;
        }
    }
}
