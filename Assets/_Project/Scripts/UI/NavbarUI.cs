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
    public class NavbarUI : MonoBehaviour
    {
        public int index = 0;
        public TextMeshProUGUI tabTitle;
        [SerializeField] private CanvasGroup canvas;

        public enum FADEANIM { FADEIN, CROSSFADE }
        [SerializeField] private FADEANIM fadeAnim;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private EventReference switchTabSFX;

        [Space]

        public bool warp;
        public Button leftBtn;
        public Button rightBtn;

        [Space]

        public List<Tab> tabs = new List<Tab>();

        private void Awake()
        {
            //if (tabTitle == null) Debug.LogWarning("No tab title label assigned.", this);
            UpdateActiveTab();
        }

        private void OnEnable()
        {
            if (leftBtn != null) leftBtn.onClick.AddListener(LeftPage);
            if (rightBtn != null) rightBtn.onClick.AddListener(RightPage);

            for (int i = 0; i < tabs.Count; i++)
                tabs[i].Set(this, i);
        }

        private void OnDisable()
        {
            if (leftBtn != null) leftBtn.onClick.RemoveListener(LeftPage);
            if (rightBtn != null) rightBtn.onClick.RemoveListener(RightPage);

            for (int i = 0; i < tabs.Count; i++)
                tabs[i].Unset();
        }

        public void SetActiveTab(int i)
        {
            StartCoroutine(SetActiveTabCoroutine(i));
        }

        public IEnumerator SetActiveTabCoroutine(int i)
        {
            if (i < 0 || i >= tabs.Count) yield break;

            if (!warp)
            {
                if (leftBtn != null) leftBtn.interactable = i > 0;
                if (rightBtn != null) rightBtn.interactable = i < tabs.Count - 1;
            }

            if (fadeAnim == FADEANIM.FADEIN)
                yield return StartCoroutine(tabs[index].Fadeout());
            else if (fadeAnim == FADEANIM.CROSSFADE)
                StartCoroutine(tabs[index].Fadeout());

            if (index != i)
            {
                RuntimeManager.PlayOneShot(switchTabSFX);
            }

            index = i;
            if (tabTitle != null) tabTitle.SetText(tabs[index].name);

            yield return StartCoroutine(tabs[index].Fadein());

        }

        public void CloseAllTabs()
        {
            foreach (Tab tab in tabs)
            {
                tab.SetActive(false);
            }
        }

        public void ChangeActiveTab(int delta)
        {
            int i = index + delta;

            if (i < 0 || i >= tabs.Count)
                if (warp)
                {
                    if (i >= tabs.Count) i = 0;
                    else if (i < 0) i = tabs.Count - 1;
                }
                else return;

            SetActiveTab(i);
        }

        public void LeftPage()
        {
            ChangeActiveTab(-1);
        }

        public void RightPage()
        {
            ChangeActiveTab(1);
        }

        public void OnValidate()
        {
            index = Math.Clamp(index, -1, tabs.Count - 1);
        }

        [Button()]
        public void UpdateActiveTab()
        {
            CloseAllTabs();
            if (index >= 0 && index < tabs.Count)
                tabs[index].SetActive(true);
        }

        [System.Serializable]
        public class Tab
        {
            public string name;
            [SerializeField] private Button button;
            [field: SerializeField] public CanvasGroup canvas { get; private set; }

            private NavbarUI ctx;
            public int index { get; private set; }

            public void Set(NavbarUI ctx, int i)
            {
                this.ctx = ctx;
                index = i;

                button.onClick.AddListener(Select);
            }

            public void Unset()
            {
                button.onClick.RemoveListener(Select);
            }

            private void Select()
            {
                ctx.SetActiveTab(index);
            }

            public void SetActive(bool active)
            {
                if (canvas == null) return;

                canvas.gameObject.SetActive(active);
                canvas.alpha = active ? 1 : 0;
                canvas.blocksRaycasts = active;
            }

            public IEnumerator Fadein()
            {
                if (canvas == null) yield return null;

                canvas.gameObject.SetActive(true);
                canvas.DOFade(1, ctx.fadeDuration);

                yield return new WaitForSeconds(ctx.fadeDuration);

                canvas.blocksRaycasts = true;

            }

            public IEnumerator Fadeout()
            {
                if (canvas == null) yield return null;

                canvas.blocksRaycasts = false;
                canvas.DOFade(0, ctx.fadeDuration);

                yield return new WaitForSeconds(ctx.fadeDuration);

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
}
