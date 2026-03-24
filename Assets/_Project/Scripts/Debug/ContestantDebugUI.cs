using System;
using Mystie.Dressup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    public class ContestantDebugUI : MonoBehaviour
    {
        public ContestantData contestant;
        public TextMeshProUGUI label;
        public TMP_Dropdown dropdown;

        private void Awake()
        {
            if (contestant != null) Set(contestant);
        }

        public void Set(ContestantData contestant)
        {
            this.contestant = contestant;
            label.text = contestant.name;
        }

        private void OnEnable()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDisable()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int value)
        {
            DressupManager.SetReaction(contestant.name.ToLowerInvariant(), value - 1);
        }
    }
}
