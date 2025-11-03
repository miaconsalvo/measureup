using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mystie.UI
{
    public class MessageBoxUI : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI characterName { get; private set; }
        [field: SerializeField] public TextMeshProUGUI text { get; private set; }


        public void SetText(string text, string characterName = null)
        {
            if (this.text != null) this.text.text = text;
            if (this.characterName != null)
            {
                this.characterName.text = characterName;
                //this.characterName.gameObject.SetActive(settings.showName && !string.IsNullOrWhiteSpace(characterName));
            }
        }
    }
}
