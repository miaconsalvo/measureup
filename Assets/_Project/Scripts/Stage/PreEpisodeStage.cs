using System.Collections;
using System.Collections.Generic;
using Mystie.UI;
using UnityEngine;

namespace Mystie.Core
{
    public class TabletStage : LevelStage
    {
        public int startIndex;
        public AppNavbarUI tablet;
        public Transform sidebar;
        public bool sidebarEnabled = true;

        [Space]

        public ChatUI chatUI;
        public string nodeStart;

        protected override void OnStageEnter()
        {
            InitializeTablet();

            sidebar.gameObject.SetActive(sidebarEnabled);
            tablet.SetActiveTab(startIndex, true);

            base.OnStageEnter();
        }

        protected virtual void InitializeTablet()
        {
            tablet.Clear();

            if (nodeStart != string.Empty) chatUI.QueueConvo(nodeStart);
        }
    }
}
