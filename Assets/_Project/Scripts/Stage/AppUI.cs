using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie
{
    public class AppUI : MonoBehaviour
    {
        public AppNavbarUI appNavbarUI;
        public bool lockNavbar = false;

        public bool active { get; private set; }

        protected bool open;

        public virtual void OnOpen()
        {
            if (open == true) return;
            open = true;
            if (lockNavbar && appNavbarUI != null)
                appNavbarUI.SetNavbarEnabled(false);
        }

        public virtual void OnClose()
        {
            if (open == false) return;
            open = false;
            if (lockNavbar && appNavbarUI != null)
                appNavbarUI.SetNavbarEnabled(true);
        }

        public virtual void Clear()
        {

        }
    }
}
