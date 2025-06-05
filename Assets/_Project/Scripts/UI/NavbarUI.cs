using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using Mystie.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class NavbarUI<ITab> : NavbarBase where ITab : Tab
    {
        public TextMeshProUGUI tabTitle;
        [SerializeField] protected CanvasGroup canvas;
        public List<ITab> tabs = new List<ITab>();
        public override int TabsCount { get => tabs.Count; }

        [SerializeField] protected float fadeDuration = 0.3f;
        [SerializeField] protected EventReference switchTabSFX;

        protected override void OnEnable()
        {
            foreach (Tab tab in tabs) tab.onSetActive += SetActiveTab;
            for (int i = 0; i < tabs.Count; i++) tabs[i].Set(i);

            UpdateActiveTab();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            foreach (Tab tab in tabs) tab.onSetActive -= SetActiveTab;

            for (int i = 0; i < tabs.Count; i++) tabs[i].Unset();
            base.OnDisable();
        }

        public override IEnumerator SetActiveTabCoroutine(int i)
        {
            if (i < 0 || i >= tabs.Count) yield break;

            canvas.DOFade(0, fadeDuration);
            canvas.blocksRaycasts = false;
            yield return new WaitForSeconds(fadeDuration);
            tabs[index].SetActive(false);

            if (index != i)
            {
                RuntimeManager.PlayOneShot(switchTabSFX);
            }

            index = i;
            if (tabTitle != null) tabTitle.SetText(tabs[index].name);

            canvas.DOFade(1, fadeDuration);
            tabs[index].SetActive(true);
            yield return new WaitForSeconds(fadeDuration);
            canvas.blocksRaycasts = true;
        }

        [Button()]
        public override void UpdateActiveTab()
        {
            CloseAllTabs();
            if (index >= 0 && index < TabsCount)
                tabs[index].SetActive(true);
        }

        public override void CloseAllTabs()
        {
            foreach (ITab tab in tabs)
            {
                tab.SetActive(false);
            }
        }
    }

    public class NavbarUI : NavbarUI<Tab> { }

    [System.Serializable]
    public class Tab
    {
        public Action<int> onSetActive;

        public string name;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button button;

        public int index { get; private set; }

        public void Set(int i)
        {
            index = i;

            button.onClick.AddListener(Select);
        }

        public void Unset()
        {
            button.onClick.RemoveListener(Select);
        }

        private void Select()
        {
            onSetActive?.Invoke(index);
        }

        public void SetActive(bool active)
        {
            if (panel != null) panel.SetActive(active);
        }

        /*
        public void Lock()
        {
            if (ui != null) ui.Lock();
        }

        public void Unlock()
        {
            if (ui != null) ui.Unlock();
        }

        public void SetComplete()
        {
            if (ui != null) ui.SetComplete();
            if (panel != null)
                panel.interactable = false;
        }

        public void ResetTab()
        {
            if (ui != null) ui.ResetState();
            if (panel != null)
                panel.interactable = true;
        }*/
    }
}
