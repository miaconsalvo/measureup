using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public abstract class NavbarBase : MonoBehaviour
    {
        [field: SerializeField] public int index { get; protected set; } = 0;
        [field: SerializeField] public bool warp { get; protected set; }

        public Button leftBtn;
        public Button rightBtn;

        public abstract int TabsCount { get; }

        protected virtual void OnEnable()
        {
            if (leftBtn != null) leftBtn.onClick.AddListener(() => ChangeActiveTab(-1));
            if (rightBtn != null) rightBtn.onClick.AddListener(() => ChangeActiveTab(1));

            UpdateActiveTab();
        }

        protected virtual void OnDisable()
        {
            if (leftBtn != null) leftBtn.onClick.RemoveAllListeners();
            if (rightBtn != null) rightBtn.onClick.RemoveAllListeners();
        }

        public void ChangeActiveTab(int delta)
        {
            int i = index + delta;

            if (i < 0 || i >= TabsCount)
                if (warp)
                {
                    if (i >= TabsCount) i = 0;
                    else if (i < 0) i = TabsCount - 1;
                }
                else return;

            SetActiveTab(i);
        }

        public void SetActiveTab(int i)
        {
            i = ConstrainIndex(i);
            if (i < 0 || i >= TabsCount) return;

            if (leftBtn != null) leftBtn.interactable = warp || i > 0;
            if (rightBtn != null) rightBtn.interactable = warp || i < TabsCount - 1;

            StartCoroutine(SetActiveTabCoroutine(i));
        }

        public int ConstrainIndex(int i)
        {
            if (warp)
            {
                if (i < 0) i = Math.Max(0, TabsCount - 1);
                else if (i >= TabsCount) i = 0;
            }
            else
            {
                if (i < 0) i = 0;
                else if (i >= TabsCount) i = Math.Max(0, TabsCount - 1);
            }

            return i;
        }

        public abstract IEnumerator SetActiveTabCoroutine(int i);

        public abstract void CloseAllTabs();

        public abstract void UpdateActiveTab();

        public void OnValidate()
        {
            index = Math.Max(0, Math.Clamp(index, -1, TabsCount - 1));
        }
    }
}
