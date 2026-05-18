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
        [SerializeField] protected Button skipButton;

        public bool active { get; private set; }
        protected bool fastForward = false;

        protected bool open;

        public virtual void OnOpen()
        {
            if (open == true) return;
            open = true;
            if (lockNavbar && appNavbarUI != null)
                appNavbarUI.SetNavbarEnabled(false);
            SetSkipButtonActive(true);
        }

        public virtual void OnClose()
        {
            if (open == false) return;
            open = false;
            if (lockNavbar && appNavbarUI != null)
                appNavbarUI.SetNavbarEnabled(true);

            SetSkipButtonActive(false);
            fastForward = false;
        }

        public virtual void Clear()
        {

        }

        public virtual void OnDisplayDone()
        {
            if (appNavbarUI != null && lockNavbar) appNavbarUI.SetNavbarEnabled(true);
            SetSkipButtonActive(false);
            fastForward = false;
        }

        protected void SetSkipButtonActive(bool active)
        {
            if (skipButton != null)
            {
                skipButton.gameObject.SetActive(active);
                if (active) skipButton.onClick.AddListener(OnFastForward);
                else skipButton.onClick.RemoveListener(OnFastForward);
            }
        }

        protected virtual void OnFastForward()
        {
            fastForward = !fastForward;
        }
    }
}
