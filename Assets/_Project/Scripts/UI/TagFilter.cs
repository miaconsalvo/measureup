using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mystie.Core;
using NaughtyAttributes;
using UnityEngine;

namespace Mystie.Dressup
{
    [Serializable]
    public class TagFilter
    {
        public event Action onUpdate;

        public bool requireAll;

        [field: SerializeField] public List<ClothingTag> filterTags { get; private set; }

        public bool HasFilterTags => filterTags.Count > 0;

        public void Set(ClothingTag tag, bool value)
        {
            if (tag == null) return;

            if (value && !filterTags.Contains(tag)) filterTags.Add(tag);
            else if (!value && filterTags.Contains(tag)) filterTags.Remove(tag);

            onUpdate?.Invoke();
        }

        public void ApplyFilter(ref Dictionary<ItemUI, bool> itemsUI)
        {
            foreach (ItemUI ui in itemsUI.Keys.ToList())
            {
                if (!itemsUI[ui]) continue;

                if (!HasFilterTags)
                {
                    itemsUI[ui] = true;
                    continue;
                }

                List<ClothingTag> tags = ui.Tags;
                bool show = requireAll;

                foreach (ClothingTag tag in filterTags)
                {
                    if (!requireAll && tags.Contains(tag))
                    {
                        show = true;
                        break;
                    }
                    else if (requireAll && !tags.Contains(tag))
                    {
                        show = false;
                        break;
                    }
                }

                itemsUI[ui] = show;
            }
        }

        [Button]
        public void Clear()
        {
            filterTags.Clear();
        }
    }
}
