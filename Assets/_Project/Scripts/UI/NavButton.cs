using Mystie.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    [System.Serializable]
    public class NavButton
    {
        [SerializeField] private string name;
        [SerializeField] public Button btn;
        [SerializeField] public UIState state;

        public void Sub(UIManager manager)
        {
            if (btn != null) btn.onClick.AddListener(() => { manager.SetState(state); });
        }

        public void Unsub(UIManager manager)
        {
            if (btn != null) btn.onClick.RemoveListener(() => { manager.SetState(state); });
        }
    }
}
