using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

namespace Mystie
{
    public class AppNavbarUI : NavbarUI<AppTab>
    {
        public void Clear()
        {
            foreach (AppTab tab in tabs)
            {
                tab.app.Clear();
            }
        }
    }

    [Serializable]
    public class AppTab : Tab
    {
        public AppUI app;

        public override void SetActive(bool active)
        {
            //Debug.Log(app.gameObject.name + ": " + active);
            base.SetActive(active);
            if (active) app.OnOpen();
            else app.OnClose();
        }
    }
}
