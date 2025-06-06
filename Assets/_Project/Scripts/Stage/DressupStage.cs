using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Mystie.Dressup
{
    public class DressupStage : LevelStage
    {
        public event Action<List<ItemScriptable>> onItemListUpdate;
        public event Action<List<ItemUI>> onItemUIListUpdate;

        public ContestantData contestant;

        [SerializeField] private DressupManager model;
        [SerializeField] private Transform itemAnchor;
        [SerializeField] private ItemUI itemPrefab;

        [SerializeField] private ItemDetailsPanelUI itemDetailsPanelUI;

        [Space]

        [SerializeField] private Button fitCheckButton;
        [SerializeField] private int fitChecksMax = 3;
        private int fitChecksDone;

        [Space]

        [SerializeField] private LabelUI opinionBox;
        [SerializeField] private float opinionDuration = 4f;

        [Space]

        [SerializeField] private List<LocalizedString> wrapupComments;
        [SerializeField] private LocalizedString inappropriateFitComment;

        private List<ItemScriptable> clothes;
        private List<ItemUI> itemsUI;

        private void Start()
        {
            if (itemDetailsPanelUI != null) itemDetailsPanelUI.Hide(false);
            UpdateItems();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            fitCheckButton.onClick.AddListener(OnFitCheck);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            fitCheckButton.onClick.RemoveListener(OnFitCheck);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (ItemUI ui in itemsUI.ToList())
            {
                DestroyItemUI(ui);
            }
        }

        protected override void OnStageEnter()
        {
            fitChecksDone = 0;
            UpdateItems();
            base.OnStageEnter();
        }

        [Button()]
        public void UpdateItems()
        {
            if (itemsUI == null) itemsUI = new List<ItemUI>();
            clothes = InventoryManager.Instance.clothes;

            for (int i = 0; i < clothes.Count; i++)
            {
                if (i < itemsUI.Count) itemsUI[i].Set(clothes[i]);
                else CreateItemUI(clothes[i]);
            }

            for (int i = clothes.Count; i < itemsUI.Count; i++)
            {
                itemsUI[i].Set(null);
                itemsUI[i].gameObject.SetActive(false);
            }

            onItemListUpdate?.Invoke(clothes);
            onItemUIListUpdate?.Invoke(itemsUI);
        }

        private ItemUI CreateItemUI(ItemScriptable c)
        {
            ItemUI itemUI = Instantiate(itemPrefab.gameObject, itemAnchor).GetComponent<ItemUI>();
            itemUI.Init(c, OnItemSelected, model.AddItem, (item) => { model.RemoveItem(item); });
            itemsUI.Add(itemUI);
            return itemUI;
        }

        private void DestroyItemUI(ItemUI itemUI)
        {
            itemsUI.Remove(itemUI);
            Destroy(itemUI.gameObject);
        }

        private void OnItemSelected(ItemScriptable item)
        {
            StartCoroutine(OnItemSelectedCoroutine(item));
        }

        public IEnumerator OnItemSelectedCoroutine(ItemScriptable item)
        {
            if (itemDetailsPanelUI == null || itemDetailsPanelUI.item == item) yield break;

            if (itemDetailsPanelUI.item != null)
            {
                itemDetailsPanelUI.Hide();
                yield return new WaitForSeconds(itemDetailsPanelUI.fadeTime);
            }

            if (itemDetailsPanelUI.item != item)
            {
                itemDetailsPanelUI.Set(item);
                if (item != null) itemDetailsPanelUI.Show();
            }
            else
            {
                itemDetailsPanelUI.Set(null);
            }
        }

        [Button]
        public void OnFitCheck()
        {
            string comment = null;
            if (!model.IsFitAppropriate())
            {
                comment = inappropriateFitComment.GetLocalizedString();
            }
            else if (fitChecksDone < fitChecksMax)
            {
                fitChecksDone++;
                comment = model.FitCheck();
            }
            else
            {
                int rand = UnityEngine.Random.Range(0, wrapupComments.Count);
                comment = wrapupComments[rand].GetLocalizedString();
            }

            if (opinionBox != null || string.IsNullOrEmpty(comment))
                StartCoroutine(ShowOpinionCoroutine(comment));
        }

        public IEnumerator ShowOpinionCoroutine(string opinion)
        {
            if (opinionBox.isVisible)
            {
                opinionBox.StopAllCoroutines();
                opinionBox.Hide();
                yield return new WaitForSeconds(opinionBox.fadeTime);
            }

            opinionBox.Set(opinion);
            opinionBox.ShowForDuration(opinionDuration);
        }

        [Button]
        public void Validate()
        {

        }
    }
}
