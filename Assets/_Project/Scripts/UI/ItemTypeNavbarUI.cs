using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using Mystie.Dressup;
using Mystie.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Mystie.UI
{
    public class ItemTypeNavbarUI : NavbarUI<ItemTypeTab>
    {
        public TagFilterUI filter;

        public override IEnumerator SetActiveTabCoroutine(int i)
        {
            if (i < 0 || i >= tabs.Count) yield break;

            canvas.DOFade(0, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);

            if (index != i)
            {
                RuntimeManager.PlayOneShot(switchTabSFX);
            }

            index = i;
            if (tabTitle != null) tabTitle.SetText(tabs[index].name);

            if (tabs[index].none) filter.typeFilter.Clear();
            else filter.typeFilter.Set(tabs[index].itemType);

            canvas.DOFade(1, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }
    }

    [Serializable]
    public class ItemTypeTab : Tab
    {
        public bool none;
        [AllowNesting, HideIf("none")] public ItemType itemType;
    }
}
