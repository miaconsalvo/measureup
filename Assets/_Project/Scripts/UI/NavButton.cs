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
        private UIManager manager;

        [SerializeField] private string name;
        [SerializeField] public Button btn;
        [SerializeField] public UIState state;
        public enum NavType {ADD, REPLACE, CLEAR}
        [SerializeField] private NavType navType;

        public void Sub(UIManager manager)
        {
            this.manager = manager;
            if (btn != null) btn.onClick.AddListener(OnClick);
        }

        public void Unsub()
        {
            if (btn != null) btn.onClick.RemoveListener(OnClick);
        }

        public void OnClick(){
            switch(navType)
            {
                case NavType.REPLACE:
                    manager.CloseState();
                    break;
                case NavType.CLEAR:
                    manager.ClearStates();
                    break;
            }
            
            manager.SetState(state);
        }
    }
}
