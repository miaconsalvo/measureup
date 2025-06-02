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
    public class NavbarUI<ITab> : NavbarUIBase where ITab : Tab
    {

        public TextMeshProUGUI tabTitle;
        [SerializeField] protected CanvasGroup canvas;

        public enum FADEANIM { FADEIN, CROSSFADE }
        [SerializeField] protected FADEANIM fadeAnim;
        [SerializeField] protected float fadeDuration = 0.3f;
        [SerializeField] protected EventReference switchTabSFX;

        [Space]

        public List<ITab> tabs = new List<ITab>();
        public override int TabCount { get => tabs.Count; }

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

            if (!warp)
            {
                if (leftBtn != null) leftBtn.interactable = i > 0;
                if (rightBtn != null) rightBtn.interactable = i < tabs.Count - 1;
            }

            if (fadeAnim == FADEANIM.FADEIN)
                yield return StartCoroutine(tabs[index].Fadeout(fadeDuration));
            else if (fadeAnim == FADEANIM.CROSSFADE)
                StartCoroutine(tabs[index].Fadeout(fadeDuration));

            if (index != i)
            {
                RuntimeManager.PlayOneShot(switchTabSFX);
            }

            index = i;
            if (tabTitle != null) tabTitle.SetText(tabs[index].name);

            yield return StartCoroutine(tabs[index].Fadein(fadeDuration));

        }

        [Button()]
        public override void UpdateActiveTab()
        {
            CloseAllTabs();
            if (index >= 0 && index < TabCount)
                tabs[index].SetActive(true);
        }

        public override void CloseAllTabs()
        {
            foreach (Tab tab in tabs)
            {
                tab.SetActive(false);
            }
        }




    }

    [System.Serializable]
    public class Tab
    {
        public Action<int> onSetActive;

        public string name;
        [SerializeField] private Button button;
        [field: SerializeField] public CanvasGroup canvas { get; private set; }

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
            if (canvas == null) return;

            canvas.gameObject.SetActive(active);
            canvas.alpha = active ? 1 : 0;
            canvas.blocksRaycasts = active;
        }

        public IEnumerator Fadein(float fadeDuration)
        {
            if (canvas == null) yield return null;

            canvas.gameObject.SetActive(true);
            canvas.DOFade(1, fadeDuration);

            yield return new WaitForSeconds(fadeDuration);

            canvas.blocksRaycasts = true;

        }

        public IEnumerator Fadeout(float fadeDuration)
        {
            if (canvas == null) yield return null;

            canvas.blocksRaycasts = false;
            canvas.DOFade(0, fadeDuration);

            yield return new WaitForSeconds(fadeDuration);

            canvas.gameObject.SetActive(false);
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
